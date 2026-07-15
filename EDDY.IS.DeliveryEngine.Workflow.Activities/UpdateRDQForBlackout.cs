using System;
using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class UpdateRDQForBlackout : CodeActivity
    {
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public InArgument<DateTime> NextDeliveryAttemptDatetime { get; set; }
        public OutArgument<bool> UpdateRDQForBlackoutStatus { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            DateTime nextDeliveryAttemptDatetime = NextDeliveryAttemptDatetime.Get(context);


            //If this is first attempt to blackout endpint, set last try time to now
            if (rdq.LastDeliveryAttemptDatetime == DateTime.MinValue)
                rdq.LastDeliveryAttemptDatetime = DateTime.Now.ToUniversalTime();

            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid(); //temp, change this

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Update RDQ record (status/next attempt datetime)
            bool updateSuccess = DeliveryEngineDataService.DeliveryEngineDAO.UpdateRealtimeDeliveryQueue(rdq.RealtimeDeliveryQueueId, 116, machineKey,
                                                                 rdq.CurrentDeliveryAttempts,
                                                                 rdq.LastDeliveryAttemptDatetime,
                                                                 nextDeliveryAttemptDatetime, 1);

            //Temp Output for debugging
            Console.WriteLine(rdq.LeadId + "-ep:" + rdq.EndpointId.ToString() +
                              " - Updated Status to blacked out in RDQ. Next delivery Attempt: " +
                              nextDeliveryAttemptDatetime.ToString());
            UpdateRDQForBlackoutStatus.Set(context, updateSuccess);


        }
    }
}
