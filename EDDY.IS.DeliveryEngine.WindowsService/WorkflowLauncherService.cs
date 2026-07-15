using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using EDDY.Nexus.Common.ExceptionHandler;
//using EDDY.Nexus.DataAccess.CoreBusiness;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;
using System.Configuration;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.WindowsService.RetryLeadDeliveryWorkflowService;
using EDDY.IS.DeliveryEngine.WindowsService.LeadProcessingDeliveryWorkflowService;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.WindowsService
{
    public partial class WorkflowLauncherService : ServiceBase
    {
        bool isDebugMode = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebugMode"]);

        readonly LeadProcessingWorkflowHost _leadProcessingHost;
        readonly RetryLeadProcessingWorkflowHost _retryLeadProcessingHost;


        Thread _worker;
        readonly AutoResetEvent _stop = new AutoResetEvent(false);

        private static readonly object Locker = new object();
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
            InitializeComponent();

            // create hosts 
            _leadProcessingHost = new LeadProcessingWorkflowHost();
            _retryLeadProcessingHost = new RetryLeadProcessingWorkflowHost();
            _machineName = GetMachineName();            
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
        protected override void OnStart(string[] args)
        {
            try
            {
                if (isDebugMode) EventLog.WriteEntry("EDDY", "Delivery Service starting...");

                //_leadProcessingHost.Start();

                // Start the WorkFlow Processing Thread 
                _worker = new Thread(ProcessDeliveryWorkflow);
                _worker.Start();

                if (isDebugMode) EventLog.WriteEntry("EDDY", "Delivery Service started successfully!");
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
        public  void Start()
        {
            OnStart(null);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
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
                if (isDebugMode) EventLog.WriteEntry("EDDY", "ProcessDeliveryWorkflow started...");

                var isBeta = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBeta"]);
                

                bool doNewLeadDeliveries;
                doNewLeadDeliveries =!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_RealTime_Lead_Delivery"])?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_RealTime_Lead_Delivery"]):true;

                bool doRetries;
                doRetries=!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_Retry_Lead_Delivery"])?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_Retry_Lead_Delivery"]):false;

                bool doThirdPartyProducts;
                doThirdPartyProducts = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Process_ThirdParty_Delivery"]) ?
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Process_ThirdParty_Delivery"]):false;

                int numParallelProcesses;
                numParallelProcesses = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Number_Parallel_Processes"]) ? 
                        Convert.ToInt32(ConfigurationManager.AppSettings["Number_Parallel_Processes"]) : 4;


                if (isDebugMode) EventLog.WriteEntry("EDDY", "ProcessDeliveryWorkflow entering the while loop...");

                // Worker thread loop 
                for (; ; )
                {
                    // Run this code once every 90 seconds = 90000, 1=1ms or stop right away if the service is stopped 
                    //on stop it will return, on 90 seconds timeout it will fall thru to CallWorkflowViaProxy
                    if (_stop.WaitOne(10)) return;

                    //Pause for 5 sec;
                    Thread.Sleep(5000);

                    try
                    {
                        if (isDebugMode) EventLog.WriteEntry("EDDY", "Database call to retrieve leads pending delivery.");

                        //NEW LEADS
                        if (doNewLeadDeliveries)
                        {
                            _leadsList = DeliveryEngineDataService.DeliveryEngineDAO.GetNewLeadsForProcessing(numParallelProcesses, _machineName, true, isBeta);
                            if (isDebugMode) EventLog.WriteEntry("EDDY", "doNewLeadDeliveries: returned leads count - " + _leadsList.Count.ToString());
                        }

                        //REPOST
                        if (doRetries)
                        {
                            _realTimeDeliveryQueueList = DeliveryEngineDataService.DeliveryEngineDAO.GetRDQsForProcessingReposts(numParallelProcesses, _machineName, true);
                            if (isDebugMode) EventLog.WriteEntry("EDDY", "doRetries: returned leads count - " + _realTimeDeliveryQueueList.Count.ToString());
                        }

                        //THIRD PARTY DELIVERY
                        if (doThirdPartyProducts)
                        {
                            _thirdPartyList = DeliveryEngineDataService.DeliveryEngineDAO.GetThirdPartyLeadsForProcessing(numParallelProcesses, _machineName, isBeta);
                            if (isDebugMode) EventLog.WriteEntry("EDDY", "doThirdPartyProducts: returned leads count - " + _thirdPartyList.Count.ToString());
                        }

                        if (isDebugMode) EventLog.WriteEntry("EDDY", "Database call to retrieve leads pending delivery completed successfully.");
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (isDebugMode) EventLog.WriteEntry("EDDY", ex.ToString());

                        //LOG THE EXEPTION
                        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
                        
                        //Pause for 5 sec;
                        Thread.Sleep(5000);

                        //if sql exception..re-establish sql connection                        
                    }
                    catch (Exception ex)
                    {
                        ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
                        
                    }



                    if ((_leadsList != null) && (_leadsList.Count != 0))
                    {
                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallLeadsDeliveryWorkflow called..");

                        CallLeadsDeliveryWorkflow(_leadsList);

                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallLeadsDeliveryWorkflow completed successfully.");
                    }


                    if ((_realTimeDeliveryQueueList != null) && (_realTimeDeliveryQueueList.Count != 0))
                    {
                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallRetryLeadsDeliveryWorkflow called..");

                        PrepareRetryList();
                        CallRetryLeadsDeliveryWorkflow();

                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallRetryLeadsDeliveryWorkflow completed successfully.");

                    }

                    if ((_thirdPartyList != null) && (_thirdPartyList.Count != 0))
                    {
                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallLeadsDeliveryWorkflow-thirdparty called..");

                        CallLeadsDeliveryWorkflow(_thirdPartyList);

                        if (isDebugMode) EventLog.WriteEntry("EDDY", "CallLeadsDeliveryWorkflow-thirdparty completed successfully.");
                    }

                }

                if (isDebugMode) EventLog.WriteEntry("EDDY", "ProcessDeliveryWorkflow completed!");
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
                _leadData = LeadDataService.LeadDAO.GetDeliveryLeadDataByLeadId(rtdqi.LeadId);

                //Get Endpoint
                _deliveryEndPoint = DeliveryEngineDataService.DeliveryEngineDAO.GetEndpointById(rtdqi.EndpointId);

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
            LeadProcessingDeliveryWorkflowServiceClient client = null;
            
            try
            {
                lock (Locker)
                {
                    client = new LeadProcessingDeliveryWorkflowServiceClient();
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
            RetryLeadDeliveryWorkflowServiceClient client = null;

            try
            {
                lock (Locker)
                {
                    client = new RetryLeadDeliveryWorkflowServiceClient();
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



        //private void InsertTransactionDetails(List<DeliveryLeadData> leadList, int activityStepId)
        //{
        //    IProductProcessingTransactionManager transactionManager = new ProductProcessingTransactionManager();

        //    foreach (var leadData in leadList)
        //    {
        //      transactionManager.InsertTransactionDetail(leadData.TransactionId, activityStepId, string.Empty);
        //    }
        //}

        //private void InsertRetryTransactionDetails(List<RealtimeDeliveryQueueItem> leadList, int activityStepId)
        //{
        //    IProductProcessingTransactionManager transactionManager = new ProductProcessingTransactionManager();

        //    foreach (RealtimeDeliveryQueueItem leadData in leadList)
        //    {
        //        var leadDao = new LeadDAO();
        //        var dld = leadDao.GetDeliveryLeadDataByLeadId(leadData.LeadId);
        //        transactionManager.InsertTransactionDetail(dld.TransactionId, activityStepId, string.Empty);
        //    }
        //}

    


    }
}
