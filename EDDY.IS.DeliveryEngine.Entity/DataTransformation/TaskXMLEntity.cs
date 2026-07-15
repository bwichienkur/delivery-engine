using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
        [DataContract]
    public class TaskXmlEntity : CommonEntity, ITaskXMLEntity
    {
       #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskXmlEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public TaskXmlEntity(IDataReader dataReader)
            : this()
        {
                     
            if (dataReader != null)
            {
                TaskXmlId = DatabaseUtilities.SetInt32Value(dataReader[(int)TaskXMLTableField.TaskXMLId]);
                TaskXml = DatabaseUtilities.SetStringValue(dataReader[(int)TaskXMLTableField.TaskXML]);
                TaskDescription = DatabaseUtilities.SetStringValue(dataReader[(int)TaskXMLTableField.TaskDescription]);
                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader[(int)TaskXMLTableField.IsEnabled]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)TaskXMLTableField.CreateDate], false);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)TaskXMLTableField.CreatedBy]);
                UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)TaskXMLTableField.UpdateDate], false);
                UpdatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)TaskXMLTableField.UpdatedBy]);
                RowGuid = DatabaseUtilities.SetGuidValue(dataReader[(int)TaskXMLTableField.RowGuid]);
            }
                      
        }
        #endregion

        #region Properties

        /// <summary>
        /// To get and set TaskXmlId
        /// </summary>
        [DataMember]
        public int TaskXmlId { get; set; }

        /// <summary>
        /// To get and set TaskXml
        /// </summary>
        [DataMember]
        public string TaskXml { get; set; }

        /// <summary>
        /// To get and set TaskDescription
        /// </summary>
        [DataMember]
        public string TaskDescription { get; set; }

        #endregion

    }
}
