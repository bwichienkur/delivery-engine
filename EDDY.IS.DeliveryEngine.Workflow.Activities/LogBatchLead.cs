using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
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

            DeliveryEngineDataService.DeliveryEngineDAO.LogBatchLead(leadId, batchId, statusId);
        }
    }
}
