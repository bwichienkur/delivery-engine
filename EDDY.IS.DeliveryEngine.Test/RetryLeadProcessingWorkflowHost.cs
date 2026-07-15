using System;
using System.ServiceModel.Activities;
using System.Xaml;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;

namespace EDDY.IS.DeliveryEngine.Test
{
    class RetryLeadProcessingWorkflowHost
    {                
        private static WorkflowServiceHost _hostRetryLeadProcessing = null;

        public RetryLeadProcessingWorkflowHost()
        {
            //try
            //{
            //    CreateServiceHost("LeadProcessingDeliveryWorkflowService.xamlx");

            //}
            ////catch (Exception ex)
            ////{
            ////    ExceptionPolicy.HandleException(
            ////    CheetahExceptionHelper.AddArgumentsToException(ex, 2, MessageType.FatalError, "WorkflowHost"),
            ////    CheetahExceptionPolicyHelper.BUSINESS_POLICY);
            ////}
            //catch (Exception ex)
            //{
            //    EventLog.WriteEntry("EDDY", ex.ToString());
            //}

        }

        public void Start()
        {
            try
            {                
                //Prepare Retry Lead Processing Host
                Uri baseAddress = new Uri(ConfigurationManager.AppSettings["RetryServiceXAMLBaseAddress"]);

                string fullFilePath = ConfigurationManager.AppSettings["RetryServiceXAMLDirectory"] + "RetryLeadDeliveryWorkflowService.xamlx";

                _hostRetryLeadProcessing = new WorkflowServiceHost(XamlServices.Load(fullFilePath), baseAddress);

                _hostRetryLeadProcessing.AddServiceEndpoint("IRetryLeadDeliveryWorkflowService",
                                        new BasicHttpBinding(),
                                        baseAddress);

                _hostRetryLeadProcessing.Open();
                                
                //var ep = host.Description.Endpoints[0];

                //Console.WriteLine("Contract: {0}", ep.Contract.Name);
                //Console.WriteLine("    at {0}", ep.Address);

            }
            //catch (Exception ex)
            //{
            //    ExceptionPolicy.HandleException(
            //    CheetahExceptionHelper.AddArgumentsToException(ex, 2, MessageType.FatalError, "Start"),
            //    CheetahExceptionPolicyHelper.BUSINESS_POLICY);
            //}
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
            }

        }

        public void Stop()
        {
            try
            {
                if (_hostRetryLeadProcessing != null)
                    _hostRetryLeadProcessing.Close();
            }
            //catch (Exception ex)
            //{
            //    ExceptionPolicy.HandleException(
            //    CheetahExceptionHelper.AddArgumentsToException(ex, 2, MessageType.FatalError, "Stop"),
            //    CheetahExceptionPolicyHelper.BUSINESS_POLICY);
            //}
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
            }

        }

        //private static WorkflowServiceHost CreateServiceHost(String xamlxName)
        //{           
        //    WorkflowService wfService = LoadService(xamlxName);
        //    WorkflowServiceHost host = new WorkflowServiceHost(wfService);

        //    //host.WorkflowExtensions.Add(new ServiceLibrary.OrderUtilityExtension());

        //    _hosts.Add(host);

        //    return host;
        //}

//        private static WorkflowService LoadService(String xamlxName)
//        {
////            String fullFilePath = Path.Combine(
//  //              @"../../EDDY.Cheetah.DeliveryEngine.DeliveryService", xamlxName);            

//            string fullFilePath = ConfigurationManager.AppSettings["ServiceXAMLDirectory"] + xamlxName;

//            try
//            {

//                //throw new NullReferenceException(String.Format("Unable to load service definition from {0}", fullFilePath));


//                //WorkflowService service =
//                //    XamlServices.Load(fullFilePath) as WorkflowService;
//                //if (service != null)
//                //{
//                //    return service;
//                //}
//                //else
//                //{
//                //    throw new NullReferenceException(String.Format(
//                //        "Unable to load service definition from {0}", fullFilePath));
//                //}
//            }
//            //catch (Exception ex)
//            //{
//            //    ExceptionPolicy.HandleException(
//            //    CheetahExceptionHelper.AddArgumentsToException(ex, 2, MessageType.FatalError, "LoadService"),
//            //    CheetahExceptionPolicyHelper.BUSINESS_POLICY);
//            //}
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry("EDDY", ex.ToString());
//            }


//            return null;
//        }

    }
}
