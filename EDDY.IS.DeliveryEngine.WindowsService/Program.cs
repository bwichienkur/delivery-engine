using System;
using System.ServiceProcess;
using EDDY.IS.DeliveryEngine.WindowsService.BatchLeadProcessingWorkflowService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            if (args == null || args.Length < 2) // no arguments - start scraping
            {
                //var a = new WorkflowLauncherService();
                //a.Start();
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                    { 
                        new WorkflowLauncherService() 
                    };

                ServiceBase.Run(ServicesToRun);
            }
            else // argument specified - process custom command
            {
                switch (args[0])
                {
                    case "-b":
                        var deliveryEndPointId = 0;
                        var productId = 0;
                        int.TryParse(args[1], out deliveryEndPointId);
                        int.TryParse(args[2], out productId);
                        Console.WriteLine("Starting Batch delivery. Endpoint id: " + deliveryEndPointId);
                        if (deliveryEndPointId != 0)
                        {
                            var lpClient = new BatchLeadDeliveryWorkflowServiceClient();
                            lpClient.processBatch(deliveryEndPointId, productId);
                            Console.WriteLine("Batch Delivery Complete");
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown argument passed:" + args[0]);
                        break;
                }
            }
            //#else

            //int newLeadsThreadCount = 1;
            //string machineKeyString = System.Configuration.ConfigurationManager.AppSettings["MachineKey"];
            //Guid machineKey = new Guid(machineKeyString);

            //    IDeliveryEngineDAO deliveryEngineDAO = new DeliveryEngineDAO();
            //    List<DeliveryLeadData> deliveryLeadDataList = deliveryEngineDAO.GetNewLeadsForProcessing(newLeadsThreadCount, machineKey, true);
            //    LeadProcessingDeliveryWorkflowServiceClient lpClient = new LeadProcessingDeliveryWorkflowServiceClient();
            //    //Loop through list and call ProcessLeads
            //    foreach (DeliveryLeadData deliveryLeadData in deliveryLeadDataList)
            //    {
            //        //ProcessLead
            //        lpClient.ProcessLeads(deliveryLeadData);
            //    };
            //#endif
        }
    }
}
