using System;
using System.Activities;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class CreateRDQItem : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> DeliveryEndpointId { get; set; }
        public OutArgument<RealtimeDeliveryQueueItem> RDQItem { get; set; }
        public InArgument<bool> IsBeta { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int deliveryEndpontId = DeliveryEndpointId.Get(context);
            RealtimeDeliveryQueueItem rdqItem = new RealtimeDeliveryQueueItem();
             bool isbeta = IsBeta.Get(context);
            if (!isbeta)//If it is real delivery
            {
          
                //change this to pull userguid from session
                Guid userGuid = Guid.NewGuid(); //temp, change this

                //Get Machine Key
                Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

                //create RDQ Record and return new RDQ Object with RDQID
                rdqItem = DeliveryEngineDataService.DeliveryEngineDAO.CreateRDQItem(leadId, deliveryEndpontId, machineKey, 1);

            }


            // Store the RDQ in the OutArgument
            RDQItem.Set(context, rdqItem);
        }
    }
}
