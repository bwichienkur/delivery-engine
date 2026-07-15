using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Test.BatchLeadProcessingWorkflowService;
using EDDY.Nexus.Common.ExceptionHandler;

namespace EDDY.IS.DeliveryEngine.Test
{

    public partial class WorkflowLauncherService
    {
        readonly LeadProcessingWorkflowHost _leadProcessingHost;
        readonly RetryLeadProcessingWorkflowHost _retryLeadProcessingHost;


        Thread _worker;
        readonly AutoResetEvent _stop = new AutoResetEvent(false);

        private static readonly object Locker = new object();
        private IDeliveryEngineDAO _deliveryEngineDao = null;
        private readonly ILeadDAO _leadDao = null;
        private DeliveryLeadData _leadData = null;
        private DeliveryEndpoint _deliveryEndPoint = null;
        private Dictionary<string, object> _realTimeDeliveryQueueDictionary = null;
        private static List<Dictionary<string, object>> _retryList = null;
        private static List<DeliveryLeadData> _leadsList = null;
        private static List<DeliveryLeadData> _thirdPartyList = null;
        private static List<RealtimeDeliveryQueueItem> _realTimeDeliveryQueueList = null;
        private readonly string _machineName = string.Empty;
        private static List<DeliveryLeadData> _previewList = null;

        public WorkflowLauncherService()
        {
            //InitializeComponent();

            // create hosts 
            _leadProcessingHost = new LeadProcessingWorkflowHost();
            _retryLeadProcessingHost = new RetryLeadProcessingWorkflowHost();
            _leadDao = new LeadDAO();
            _machineName = GetMachineName();
            _deliveryEngineDao = new DeliveryEngineDAO();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetMachineName()
        {
            return System.Environment.MachineName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected void OnStart(string[] args)
        {
            try
            {
                //_leadProcessingHost.Start();

                // Start the WorkFlow Processing Thread 
                _worker = new Thread(ProcessDeliveryWorkflow);
                _worker.Start();

                //ProcessDeliveryWorkflow(null);
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            OnStart(null);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnStop()
        {
            try
            {
                // Signal worker to stop and wait until it does 
                _stop.Set();
                _worker.Join();

                _leadProcessingHost.Stop();
                _retryLeadProcessingHost.Stop();
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void ProcessDeliveryWorkflow(object arg)
        {
            try
            {
                var isBeta = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBeta"]);

                bool doNewLeadDeliveries;
                doNewLeadDeliveries = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_RealTime_Lead_Delivery"]) ?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_RealTime_Lead_Delivery"]) : true;

                bool doRetries;
                doRetries = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_Retry_Lead_Delivery"]) ?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_Retry_Lead_Delivery"]) : false;

                bool doThirdPartyProducts;
                doThirdPartyProducts = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_ThirdParty_Delivery"]) ?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_ThirdParty_Delivery"]) : false;

                int numParallelProcesses;
                numParallelProcesses = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Number_Parallel_Processes"]) ?
                        Convert.ToInt32(ConfigurationManager.AppSettings["Number_Parallel_Processes"]) : 4;

                // Worker thread loop 
                for (;;)
                {
                    // Run this code once every 90 seconds = 90000, 1=1ms or stop right away if the service is stopped 
                    //on stop it will return, on 90 seconds timeout it will fall thru to CallWorkflowViaProxy
                    if (_stop.WaitOne(10)) return;

                    //Pause for 5 sec;
                    Thread.Sleep(5000);

                    try
                    {
                        //NEW LEADS
                        if (doNewLeadDeliveries)
                        {
                            _leadsList = _deliveryEngineDao.GetNewLeadsForProcessing(numParallelProcesses, _machineName, true, isBeta);
                        }

                        //REPOST
                        if (doRetries)
                        {
                            _realTimeDeliveryQueueList = _deliveryEngineDao.GetRDQsForProcessingReposts(numParallelProcesses, _machineName, true);
                        }

                        //THIRD PARTY DELIVERY
                        if (doThirdPartyProducts)
                        {
                            _thirdPartyList = _deliveryEngineDao.GetThirdPartyLeadsForProcessing(numParallelProcesses, _machineName, isBeta);
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        //LOG THE EXEPTION
                        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);

                        //Pause for 5 sec;
                        Thread.Sleep(5000);

                        //if sql exception..re-establish sql connection
                        _deliveryEngineDao = new DeliveryEngineDAO();
                    }
                    catch (Exception ex)
                    {
                        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);

                    }



                    if ((_leadsList != null) && (_leadsList.Count != 0))
                    {
                        CallLeadsDeliveryWorkflow(_leadsList);
                    }


                    if ((_realTimeDeliveryQueueList != null) && (_realTimeDeliveryQueueList.Count != 0))
                    {
                        PrepareRetryList();
                        CallRetryLeadsDeliveryWorkflow();
                    }

                    if ((_thirdPartyList != null) && (_thirdPartyList.Count != 0))
                    {
                        CallLeadsDeliveryWorkflow(_thirdPartyList);
                    }

                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }
        }


        private void PrepareRetryList()
        {
            _retryList = new List<Dictionary<string, object>>();

            foreach (var rtdqi in _realTimeDeliveryQueueList)
            {
                //Get Lead Data
                _leadData = _leadDao.GetDeliveryLeadDataByLeadId(rtdqi.LeadId);

                //Get Endpoint
                _deliveryEndPoint = _deliveryEngineDao.GetEndpointById(rtdqi.EndpointId);

                _realTimeDeliveryQueueDictionary = new Dictionary<string, object>
                                                       {
                                                           {"RDQItem", rtdqi},
                                                           {"LeadData", _leadData},
                                                           {"DeliveryEndpoint", _deliveryEndPoint},
                                                           {"DeliveryEndpointId", rtdqi.EndpointId}
                                                       };

                _retryList.Add(_realTimeDeliveryQueueDictionary);
            }
        }

        static void CallLeadsDeliveryWorkflow(List<DeliveryLeadData> source)
        {
            try
            {
                if (source.Count == 0) return;
                Parallel.ForEach(source.ToArray<DeliveryLeadData>(), ExecuteServiceAction);
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }

        }


        private static void ExecuteServiceAction(DeliveryLeadData leadData)
        {
            EDDY.IS.DeliveryEngine.Test.LeadProcessingDeliveryWorkflowService.LeadProcessingDeliveryWorkflowServiceClient client = null;

            try
            {
                lock (Locker)
                {
                    client = new EDDY.IS.DeliveryEngine.Test.LeadProcessingDeliveryWorkflowService.LeadProcessingDeliveryWorkflowServiceClient();
                    client.ProcessLeads(leadData);
                }
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }
            finally
            {
                if (client != null) client.Close();
            }
        }

        static void CallRetryLeadsDeliveryWorkflow()
        {
            try
            {
                if (_retryList.Count == 0) return;
                Parallel.ForEach(_retryList.ToArray<Dictionary<string, object>>(), ExecuteRetryServiceAction);
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }

        }


        private static void ExecuteRetryServiceAction(Dictionary<string, object> retryData)
        {
            EDDY.IS.DeliveryEngine.Test.RetryLeadDeliveryWorkflowService.RetryLeadDeliveryWorkflowServiceClient client = null;

            try
            {
                lock (Locker)
                {
                    client = new EDDY.IS.DeliveryEngine.Test.RetryLeadDeliveryWorkflowService.RetryLeadDeliveryWorkflowServiceClient();
                    client.RetryProcessLeads((RealtimeDeliveryQueueItem)retryData["RDQItem"], (DeliveryLeadData)retryData["LeadData"], (int)retryData["DeliveryEndpointId"]);
                }
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }
            finally
            {
                if (client != null) client.Close();
            }
        }
    }
}
