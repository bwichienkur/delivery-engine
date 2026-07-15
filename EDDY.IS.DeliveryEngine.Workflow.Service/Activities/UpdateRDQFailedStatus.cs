using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class UpdateRDQFailedStatus : CodeActivity
    {
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public OutArgument<bool> RDQFailedStatus { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            DeliveryEndpoint endpoint = Endpoint.Get(context);

           
                //Get time until next Retry from endpoint
                TimeSpan retryTimeSpan = new TimeSpan(endpoint.RetryDelayInHours, 0, 0); //x hrs

                //Add retry TimeSpan to get new Delivery Attempt time
                DateTime newDeliveryAttemptTime = rdq.LastDeliveryAttemptDatetime.Add(retryTimeSpan);

                //change this to pull userguid from session
                Guid userGuid = Guid.NewGuid();

                //Get Machine Key
                Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

                //Update RDQ record (status/attempts/last attempt datetime/next attempt datetime)
                IDeliveryEngineDAO dao = new DeliveryEngineDAO();
                bool updateSuccess = dao.UpdateRealtimeDeliveryQueue(rdq.RealtimeDeliveryQueueId, 115, machineKey, rdq.CurrentDeliveryAttempts, rdq.LastDeliveryAttemptDatetime, newDeliveryAttemptTime, 1);

                //Temp Output for debugging
                Console.WriteLine(rdq.LeadId + "-ep:" + rdq.EndpointId.ToString() + " - Updated Status to failed in RDQ. Next POST Attempt: " + newDeliveryAttemptTime.ToString());
                RDQFailedStatus.Set(context, updateSuccess);

           
          
        }
    }
}

