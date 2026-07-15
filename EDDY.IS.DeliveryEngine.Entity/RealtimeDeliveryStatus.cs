using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class RealtimeDeliveryStatus : CommonEntity
    {
        #region Data Member

        [DataMember]
        public string RealTimeDeliveryStatusName { get; set; }
        
        [DataMember]
        public int RealTimeDeliveryStatusId { get; set; }

        #endregion

        #region Constructors
        
        public RealtimeDeliveryStatus()
        {

        }

        public RealtimeDeliveryStatus(IDataReader dataReader)
        {
            if (dataReader == null) return;
            RealTimeDeliveryStatusId = DatabaseUtilities.SetInt32Value(dataReader["RealtimeDeliveryStatusId"]);
            RealTimeDeliveryStatusName = DatabaseUtilities.SetStringValue(dataReader["RealTimeDeliveryStatusName"]);
        }

        #endregion

        #region enum

        public enum RealtimeDeliveryStatusValue
        {
            NEW = 100,
            DELIVERED = 110,
            BATCHDELIVERY_LEAD = 111,
            RETRYING_TEST = 120,
            DELIVERED_ALL_ENDPOINTS = 200,
            DELIVERED_TEST = 220,
            DELIVERY_FAILED_PERMANENTLY = 410,
            FAILED_TEST = 420,
            EDDY_FORM_VALIDATION_FAILED = 430,
            LEAD_PING_INTERNAL_DUPLICATE = 439,
            LEAD_PING_EXTERNAL_DUPLICATE = 438,
            LEAD_PING_INTERNAL_SCORE_CHECK_FAILURE = 437,
            LEAD_PING_EXTERNAL_SCORE_CHECK_FAILURE = 436,
            EDDY_FORM_INTERNAL_DUPLICATE_LEAD = 440,
            DUPLICATE_RESPONSE_DELIVERED = 450,
            FAILED_RESPONSE_DELIVERED = 460,
            EDDY_FORM_TEST_LEAD = 470,
            NEXUS_DELIVERY_NOT_REQUIRED = 900,
            LEGACY_DELIVERY = 990,
            EMD_SYNC_NOT_DELIVERED = 998,
            ELEARNERS_SYNC_NOT_DELIVERED = 999
        }

        #endregion
    }
}
