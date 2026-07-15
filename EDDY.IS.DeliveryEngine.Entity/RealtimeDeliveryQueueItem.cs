using System;
using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
     [DataContract]
    public class RealtimeDeliveryQueueItem
    {
         [DataMember]
        public int RealtimeDeliveryQueueId { get; set; }

         [DataMember]
        public int EndpointId { get; set; }

         [DataMember]
        public int LeadId { get; set; }

         [DataMember]
        public int DeliveryStatusId { get; set; }

         [DataMember]
        public Guid DeliveryEngineMachineKey { get; set; }

         [DataMember]
         public string DeliveryEngineMachineName { get; set; }

         [DataMember]
        public int CurrentDeliveryAttempts { get; set; }

         [DataMember]
        public DateTime LastDeliveryAttemptDatetime { get; set; }

         [DataMember]
        public DateTime NextDeliveryAttemptDatetime { get; set; }

    }

}