using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LVRealtimeLogEntity
    {
        private DataView view;

        [DataMember]
        public int LeadId { get; set; }
        [DataMember]
        public int RealtimeDeliveryLogID { get; set; }
        [DataMember]
        public int DeliveryEndpointID { get; set; }
        [DataMember]
        public int DeliveryDefID { get; set; }
        [DataMember]
        public DateTime DeliveryDateTime { get; set; }
        [DataMember]
        public int AttemptNumber { get; set; }
        [DataMember]
        public string PostURL { get; set; }
        [DataMember]
        public string EmailTo { get; set; }
        [DataMember]
        public int DeliveryTypeId { get; set; }
       
        
        public LVRealtimeLogEntity()
        {
            
        }
        public LVRealtimeLogEntity(IDataReader dataReader)
        {
            PostURL = DatabaseUtilities.SetStringValue(dataReader["PostURL"]);
            LeadId = DatabaseUtilities.SetInt32Value(dataReader["LeadId"]);
            RealtimeDeliveryLogID = DatabaseUtilities.SetInt32Value(dataReader["RealtimeDeliveryLogID"]);
            DeliveryEndpointID = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointID"]);
            DeliveryDefID = DatabaseUtilities.SetInt32Value(dataReader["DeliveryDefID"]);
            EmailTo = DatabaseUtilities.SetStringValue(dataReader["EmailTo"]);
            DeliveryDateTime = DatabaseUtilities.SetDateTimeValue(dataReader["DeliveryDateTime"], false);
            AttemptNumber = DatabaseUtilities.SetInt32Value(dataReader["AttemptNumber"]);
            DeliveryTypeId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryTypeId"]);
        }

        public LVRealtimeLogEntity(DataRowView drv)
        {
            PostURL = DatabaseUtilities.SetStringValue(drv["PostURL"]);
            LeadId = DatabaseUtilities.SetInt32Value(drv["LeadId"]);
            RealtimeDeliveryLogID = DatabaseUtilities.SetInt32Value(drv["RealtimeDeliveryLogID"]);
            DeliveryEndpointID = DatabaseUtilities.SetInt32Value(drv["DeliveryEndpointID"]);
            DeliveryDefID = DatabaseUtilities.SetInt32Value(drv["DeliveryDefID"]);
            EmailTo = DatabaseUtilities.SetStringValue(drv["EmailTo"]);
            DeliveryDateTime = DatabaseUtilities.SetDateTimeValue(drv["DeliveryDateTime"], false);
            AttemptNumber = DatabaseUtilities.SetInt32Value(drv["AttemptNumber"]);
            DeliveryTypeId = DatabaseUtilities.SetInt32Value(drv["DeliveryTypeId"]);
        }
    }
}
