using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryEndPointItem:CommonEntity
    {

        #region Porperties
        [DataMember]
        public int RowID { get; set; }
        [DataMember]
        public int EndPointID { get; set;}
        [DataMember]
        public string EndPointName{ get; set; }
        [DataMember]
        public int Active{ get; set; }

        [DataMember]
        public int IsPrimary { get; set; }
        [DataMember]
        public int IsInternal { get; set; }
        #endregion
        #region Constructor
        public DeliveryEndPointItem()
        {

        }

        public DeliveryEndPointItem(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                RowID = DatabaseUtilities.SetInt32Value(dataReader["RowID"]);
                EndPointID = DatabaseUtilities.SetInt32Value(dataReader["EndPointID"]);
                EndPointName = DatabaseUtilities.SetStringValue(dataReader["EndPointName"]);
                Active = (bool)(dataReader["Active"])?1:0;
                IsPrimary = (bool)(dataReader["IsPrimary"]) ? 1 : 0;
                IsInternal = (bool)(dataReader["IsInternal"]) ? 1 : 0;
            }
        }

       
        #endregion
    }
}
