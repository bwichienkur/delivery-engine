using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogBatchSent : CodeActivity
    {
        public InArgument<int> BatchId { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<string> BatchFileName { get; set; }
        public InArgument<int> LeadCount { get; set; }
        public OutArgument<bool> LogBatchDeliveryComplete { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var endpoint = Endpoint.Get(context);
            string batchFileName = BatchFileName.Get(context);
            int leadCount = LeadCount.Get(context);
            int deliveryTypeId = endpoint.DeliveryTypeId;
            int batchId = BatchId.Get(context);

            IDeliveryEngineDAO deliveryEngineDao = new DeliveryEngineDAO();

            if (deliveryEngineDao.LogBatchDeliveryComplete(batchId, endpoint, batchFileName, leadCount, 1))
            {
                Console.WriteLine("Batch: " + batchId + " - ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log Batch Sent.");
                LogBatchDeliveryComplete.Set(context, true);
            }
            else
            {
                LogBatchDeliveryComplete.Set(context, true);
            }

        }
    }
}
