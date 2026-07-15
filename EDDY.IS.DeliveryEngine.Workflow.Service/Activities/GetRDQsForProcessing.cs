using System;
using System.Activities;
using System.Collections.Generic;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetRDQsForProcessing : CodeActivity
    {
        public InArgument<Int32> ReturnRecordsCount { get; set; }
        public InArgument<bool> UpdateStatus { get; set; }
        public OutArgument<List<RealtimeDeliveryQueueItem>> RDQItems { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            int returnRecordsCount = ReturnRecordsCount.Get(context);
            bool updateStatus = UpdateStatus.Get(context);
            List<RealtimeDeliveryQueueItem> rdqItems = new List<RealtimeDeliveryQueueItem>();

          
                IDeliveryEngineDAO deliveryEngineDAO = new DeliveryEngineDAO();
                //rdqItems = deliveryEngineDAO.GetRDQsForProcessing(returnRecordsCount, updateStatus);
            

           

            //return List
            RDQItems.Set(context, rdqItems);

        }
    }
}

