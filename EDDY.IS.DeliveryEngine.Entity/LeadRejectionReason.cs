
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class LeadRejectionReasonItem 
    {

        #region Porperties               
        [DataMember]
        public int LeadRejectionReasonId { get; set; }
       
        [DataMember]
        public string LeadRejectionReasonName { get; set; }
        #endregion


        #region Constructor
        public LeadRejectionReasonItem()
        {

        }

        public LeadRejectionReasonItem(IDataReader dataReader)
        {
            if (dataReader != null)
            {
                LeadRejectionReasonId = DatabaseUtilities.SetInt32Value(dataReader["LeadRejectionReasonId"]);
                LeadRejectionReasonName = DatabaseUtilities.SetStringValue(dataReader["LeadRejectionReason"]);                
            }
        }
       
        #endregion
    }

}

