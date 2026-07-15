using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogBatchLead : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> BatchId { get; set; }
        public InArgument<int> StatusId { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int batchId = BatchId.Get(context);
            int statusId = StatusId.Get(context);

            IDeliveryEngineDAO deDao = new DeliveryEngineDAO();
            deDao.LogBatchLead(leadId, batchId, statusId);

        }
    }
}
