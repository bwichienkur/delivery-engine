using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class FinalizeLeadPreview:CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> LeadRealtimeDeliveryStatus { get; set; }
        public OutArgument<bool> RealtimeDeliveryStatusUpdated { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int leadRealtimeDeliveryStatus = LeadRealtimeDeliveryStatus.Get(context);

            //Replace this later with real user guid
            //Guid userGuid;
            //userGuid = new Guid();


            //Update Record in Lead Table with RealtimeDeliveryStatus
            bool result = DeliveryEngineDataService.DeliveryEngineDAO.FinalizeLeadDelivery(leadId, leadRealtimeDeliveryStatus, 1);

            RealtimeDeliveryStatusUpdated.Set(context, result);


        }
    }
}
