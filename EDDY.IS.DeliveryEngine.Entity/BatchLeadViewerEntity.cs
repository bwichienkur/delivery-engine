using System;
using System.Collections.Generic;
using System.Data;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class BatchLeadViewerEntity:CommonEntity
    {
        public string BatchFileName { get; set;}
        public int DeliveryEndPointID { get; set; }
        public int BatchID { get; set; }
        public int LeadCount { get; set; }
        public int TotalCount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<LVBatchLogEntity> BatchLogEntries { get; set; }
        public BatchLeadViewerEntity()
        {
            
        }
        public BatchLeadViewerEntity(IDataReader dataReader): this()
        {
            BatchFileName = DatabaseUtilities.SetStringValue(dataReader["BatchFileName"]);
            DeliveryEndPointID = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndPointId"]);
            BatchID = DatabaseUtilities.SetInt32Value(dataReader["BatchDeliveryId"]);
            LeadCount = DatabaseUtilities.SetInt32Value(dataReader["LeadCount"]);
            DeliveryDate = DatabaseUtilities.SetDateTimeValue(dataReader["DeliveryDate"], false);
            BatchFileName = DatabaseUtilities.SetStringValue(dataReader["BatchFileName"]);

        }
    }
}
