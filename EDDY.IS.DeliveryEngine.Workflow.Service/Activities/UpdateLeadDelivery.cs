using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class UpdateLeadDelivery : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> DeliveryDefinitionId { get; set; }
        public OutArgument<int> EndpointsAdded { get; set; }
        public InArgument<bool> IsBeta { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int deliveryDefinitionId = DeliveryDefinitionId.Get(context);
            int endpointsAdded = -1;
            bool isbeta = IsBeta.Get(context);
            //Update Record in Lead Table with DeliveryDefinitionId and add LeadDelivery records
            IDeliveryEngineDAO deDao = new DeliveryEngineDAO();
            endpointsAdded = deDao.CreateLeadDeliveryRecords(leadId, deliveryDefinitionId, 1, isbeta);

            // Store the endpointsAdded in the OutArgument
            EndpointsAdded.Set(context, endpointsAdded);
        }
    }
}
