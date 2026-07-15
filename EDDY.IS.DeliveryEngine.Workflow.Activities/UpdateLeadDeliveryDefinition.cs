using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class UpdateLeadDeliveryDefinition : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> DeliveryDefinitionId { get; set; }
        public OutArgument<bool> Success { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int deliveryDefinitionId = DeliveryDefinitionId.Get(context);

            //Update Record in Lead Table with DeliveryDefinitionId
            DeliveryEngineDataService.DeliveryEngineDAO.UpdateLeadDeliveryDefinition(leadId, deliveryDefinitionId, 1);


            // Store the succeess var in the OutArgument
            Success.Set(context, true);
        }
    }
}
