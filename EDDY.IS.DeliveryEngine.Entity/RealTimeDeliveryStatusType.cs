using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;


namespace EDDY.IS.DeliveryEngine.Entity
{
    public class RealTimeDeliveryStatusType : CommonEntity
    {
         #region Data Member

            [DataMember]
            public string RealtimeDeliveryStatusTypeName { get; set; }
        
            [DataMember]
            public int RealtimeDeliveryStatusTypeID { get; set; }

        #endregion

        #region Constructors
        
            public RealTimeDeliveryStatusType()
            {

            }

            public RealTimeDeliveryStatusType(IDataReader dataReader)
            {
                if (dataReader == null) return;
                RealtimeDeliveryStatusTypeID = DatabaseUtilities.SetInt32Value(dataReader["RealtimeDeliveryStatusTypeID"]);
                RealtimeDeliveryStatusTypeName = DatabaseUtilities.SetStringValue(dataReader["RealtimeDeliveryStatusTypeName"]);
            }

        #endregion

      
    }
}
