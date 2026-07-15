using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class UpdateStatusForBatchIfPrimaryDelivery : CodeActivity
    {

        public InArgument<int> LeadId { get; set; }
        public InArgument<bool> Success { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint{ get; set; }
            
        protected override void Execute(CodeActivityContext context)
        {
            bool statusUpdated=false;
            int leadId = LeadId.Get(context);
            var endpoint = Endpoint.Get(context);
            int deliveryEndpointId = endpoint.DeliveryEndpointId;
            bool isPrimary = endpoint.IsPrimary;
            bool isSuccess = Success.Get(context);

            if (isPrimary == true)
            { 
                //ONLY update the realtimestatus for batch delivery when it is the primary delivery.
                statusUpdated = DeliveryEngineDataService.DeliveryEngineDAO.UpdateBatchStatusIfPrimaryDelivery(leadId, deliveryEndpointId, isSuccess);
            }
           
        }
        
    }
}
