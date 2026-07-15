using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.Interface.DataTransformation;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    [DataContract]
    public class ConditionXmlEntity : CommonEntity, IConditionXMLEntity
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConditionXmlEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public ConditionXmlEntity(IDataReader dataReader) : this()
        {
           
            if (dataReader != null)
            {
                ConditionXmlId = DatabaseUtilities.SetInt32Value(dataReader[(int)ConditionXMLTableField.ConditionXMLId]);
                ConditionXml = DatabaseUtilities.SetStringValue(dataReader[(int)ConditionXMLTableField.ConditionXML]);
                ConditionDescription = DatabaseUtilities.SetStringValue(dataReader[(int)ConditionXMLTableField.ConditionDescription]);
                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader[(int)ConditionXMLTableField.IsEnabled]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)ConditionXMLTableField.CreateDate], false);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)ConditionXMLTableField.CreatedBy]);
                UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)ConditionXMLTableField.UpdateDate], false);
                UpdatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)ConditionXMLTableField.UpdatedBy]);
                RowGuid = DatabaseUtilities.SetGuidValue(dataReader[(int)ConditionXMLTableField.RowGuid]);
            }
            
        }
        #endregion

        #region Properties
        /// <summary>
        /// To get and set ConditionXmlId
        /// </summary>
        [DataMember]
        public int ConditionXmlId { get; set; }

        /// <summary>
        /// To get and set ConditionXml
        /// </summary>
        [DataMember]
        public string ConditionXml { get; set; }
         
        /// <summary>
        /// To get and set ConditionDescription
        /// </summary>
        [DataMember]
        public string ConditionDescription { get; set; }

        #endregion
        
    }  
}
