using System;
using System.Collections.Generic;
using System.Data;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LeadViewerHistoryItem:CommonEntity
    {
        public int LeadViewerHistoryID { get; set; }
        public int LeadID { get; set; }
        public string OriginalLeadData { get; set; }
        public string ModifiedLeadData { get; set; }
        public string UpdatedUserName { get; set; }
        public Dictionary<string,string> ModifiedFields { get; set; }
        public DateTime UpdatedDate { get; set; }

        public LeadViewerHistoryItem()
        {
            
        }
        public LeadViewerHistoryItem(IDataReader dataReader)
            : this()
        {
            this.LeadViewerHistoryID = DatabaseUtilities.SetInt32Value(dataReader["LeadViewerHistoryID"]);
            this.LeadID = DatabaseUtilities.SetInt32Value(dataReader["LeadID"]);
            this.OriginalLeadData = DatabaseUtilities.SetStringValue(dataReader["OriginalLeadData"]);
            this.ModifiedLeadData = DatabaseUtilities.SetStringValue(dataReader["ModifiedLeadData"]);
            this.UpdatedUserName = DatabaseUtilities.SetStringValue(dataReader["UpdatedUserName"]);
            this.UpdatedDate = Convert.ToDateTime(dataReader["UpdatedDate"]);
            this.ModifiedFields = XmlUtilities.GetModifiedLeadFieldInfo(this.OriginalLeadData, this.ModifiedLeadData);
        }
    }
}
