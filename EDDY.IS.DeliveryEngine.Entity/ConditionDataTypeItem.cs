using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class ConditionDataTypeItem
    {
        [DataMember]
        public string ConditionShortName { get; set;}
        [DataMember]
        public string DataTypeShortName { get; set; }

    
        #region Constructor
        public ConditionDataTypeItem()
        {

        }

        public ConditionDataTypeItem(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                ConditionShortName = DatabaseUtilities.SetStringValue(dataReader["ConditionShortName"]);
                DataTypeShortName = DatabaseUtilities.SetStringValue(dataReader["DataTypeShortName"]);
                

            }
        }

       
        #endregion
    }
}
