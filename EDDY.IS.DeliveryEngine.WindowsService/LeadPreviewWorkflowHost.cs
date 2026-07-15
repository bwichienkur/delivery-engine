using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.Text;
using System.Xaml;
using EDDY.Nexus.Common.ExceptionHandler;

namespace EDDY.IS.DeliveryEngine.WindowsService
{
    class LeadPreviewWorkflowHost
    {
          private static WorkflowServiceHost _hostLeadProcessing = null;

        public LeadPreviewWorkflowHost()
        {
           

        }

        public void Start()
        {
            try
            {                
                //Prepare Lead Processing Host
                Uri baseAddress = new Uri(ConfigurationManager.AppSettings["PreviewXAMLBaseAddress"]);

                string fullFilePath = ConfigurationManager.AppSettings["PreviewXAMLDirectory"] + "LeadPreviewDeliveryWorkflowService.xamlx";

                _hostLeadProcessing = new WorkflowServiceHost(XamlServices.Load(fullFilePath), baseAddress);

                _hostLeadProcessing.AddServiceEndpoint("LeadPreviwDeliveryWorkflowService",
                                        new BasicHttpBinding(),
                                        baseAddress);

                _hostLeadProcessing.Open();

                



                //var ep = host.Description.Endpoints[0];

                //Console.WriteLine("Contract: {0}", ep.Contract.N`ame);
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
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }

        }

        public void Stop()
        {
            try
            {
                if (_hostLeadProcessing != null)
                    _hostLeadProcessing.Close();

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
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            }

        }
    }
}
