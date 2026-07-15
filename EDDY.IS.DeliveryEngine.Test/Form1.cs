using System;
using System.ServiceModel.Activities;
using System.Xaml;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using EDDY.Nexus.Common.ExceptionHandler;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.Test.BatchLeadProcessingWorkflowService;

namespace EDDY.IS.DeliveryEngine.Test
{
    public partial class Form1 : Form
    {
        private static WorkflowServiceHost _hostLeadProcessing = null;      

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartLeadProcessing_Click(object sender, EventArgs e)
        {
            WorkflowLauncherService workflowLauncherService = new WorkflowLauncherService();
            workflowLauncherService.Start();
            //try
            //{
            //    //Prepare Lead Processing Host
            //    Uri baseAddress = new Uri(ConfigurationManager.AppSettings["ServiceXAMLBaseAddress"]);

            //    string fullFilePath = ConfigurationManager.AppSettings["ServiceXAMLDirectory"] + "LeadProcessingDeliveryWorkflowService.xamlx";
            //    Object obj = XamlServices.Load(fullFilePath);
            //    _hostLeadProcessing = new WorkflowServiceHost(obj, baseAddress);

            //    _hostLeadProcessing.AddServiceEndpoint("ILeadProcessingDeliveryWorkflowService",
            //                            new BasicHttpBinding(),
            //                            baseAddress);

            //    _hostLeadProcessing.Open();


            //    //var ep = host.Description.Endpoints[0];

            //    //Console.WriteLine("Contract: {0}", ep.Contract.N`ame);
            //    //Console.WriteLine("    at {0}", ep.Address);

            //}
            ////catch (Exception ex)
            ////{
            ////    ExceptionPolicy.HandleException(
            ////    CheetahExceptionHelper.AddArgumentsToException(ex, 2, MessageType.FatalError, "Start"),
            ////    CheetahExceptionPolicyHelper.BUSINESS_POLICY);
            ////}
            //catch (Exception ex)
            //{
            //    EventLog.WriteEntry("EDDY", ex.ToString());
            //    ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.USER_INTERFACE_POLICY_NO_RETHROW);
            //}
        }

        private void btnStartBatchProcessing_Click(object sender, EventArgs e)
        {
            var deliveryEndPointId = 36677;
            var productId = 1;
            Console.WriteLine("Starting Batch delivery. Endpoint id: " + deliveryEndPointId);
            if (deliveryEndPointId != 0)
            {
                var lpClient = new BatchLeadDeliveryWorkflowServiceClient();
                lpClient.processBatch(deliveryEndPointId, productId);
                Console.WriteLine("Batch Delivery Complete");
            }
        }
    }

}
