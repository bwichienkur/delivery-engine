using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class RemoveRDQItem : CodeActivity
    {
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public OutArgument<bool> RemoveRDQItemStatus { get; set; }
        public InArgument<bool> IsBeta { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            bool isbeta = IsBeta.Get(context);
            bool deleteSuccess = true;
            if (!isbeta)//If the delivery is not Beta
            {
            //Remove RDQ record
            IDeliveryEngineDAO dao = new DeliveryEngineDAO();
            deleteSuccess = dao.DeleteRealtimeDeliveryQueueRecord(rdq.RealtimeDeliveryQueueId);

                Console.WriteLine(rdq.LeadId + "-ep:" + rdq.EndpointId.ToString() + " - Removed from RDQ");
            }
            else Console.WriteLine(rdq.LeadId + "-ep:" + rdq.EndpointId.ToString() + " - Doesn't require to remove RDQ since it never created for Beta Delivery");
            RemoveRDQItemStatus.Set(context, deleteSuccess);
        }
    }
}

