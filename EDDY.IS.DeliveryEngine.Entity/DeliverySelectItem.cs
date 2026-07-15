using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliverySelectItem
    {
        #region Porperties
        [DataMember]
        public string TypeId { get; set;}
        [DataMember]
        public string TypeDesc { get; set; }

        #endregion
        #region Constructor
        public DeliverySelectItem()
        {

        }

        public DeliverySelectItem(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                TypeId = DatabaseUtilities.SetStringValue(dataReader["TypeId"]);
                TypeDesc = DatabaseUtilities.SetStringValue(dataReader["TypeDesc"]);
                
            }
        }

       
        #endregion
    }
}
