using System;
using System.Data;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LVBatchLogEntity:CommonEntity
    {
        public int BatchDeliveryLogID{get;set;}
        public int BatchID { get; set;}
        public int LeadID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RealtimeDeliveryStatusName { get; set; }

        public LVBatchLogEntity()
        {
            

        }
        public LVBatchLogEntity(IDataReader dataReader)
        {

            BatchDeliveryLogID = DatabaseUtilities.SetInt32Value(dataReader["BatchDeliveryLogID"]);
            LeadID = Convert.ToInt32(dataReader["LeadId"]);
            BatchID = DatabaseUtilities.SetInt32Value(dataReader["BatchId"]);
            FirstName = DatabaseUtilities.SetStringValue(dataReader["FirstName"]);
            LastName = DatabaseUtilities.SetStringValue(dataReader["LastName"]);
            EmailAddress = DatabaseUtilities.SetStringValue(dataReader["EmailAddress"]);
            RealtimeDeliveryStatusName = DatabaseUtilities.SetStringValue(dataReader["RealtimeDeliveryStatusName"]);
            
        }
    }
}
