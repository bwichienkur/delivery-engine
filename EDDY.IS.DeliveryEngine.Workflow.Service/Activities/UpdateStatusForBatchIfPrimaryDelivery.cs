using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;


namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
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
                IDeliveryEngineDAO deliveryEngineDAO = new DeliveryEngineDAO();
                statusUpdated = deliveryEngineDAO.UpdateBatchStatusIfPrimaryDelivery(leadId, deliveryEndpointId, isSuccess);
            }
           
        }
        
    }
}
