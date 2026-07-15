using System;
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
    public class DataTransformationEntityNew: CommonEntity, IDataTransformationEntityNew
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataTransformationEntityNew()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public DataTransformationEntityNew(IDataReader dataReader)
            : this()
        {
            if (dataReader != null)
            {
                Id = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.Id.ToString()]);
                Name = DatabaseUtilities.SetStringValue(dataReader[TaskTableField.Name.ToString()]);
                ConditionXmlId = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.ConditionXmlId.ToString()]);
                ConditionXml = DatabaseUtilities.SetStringValue(dataReader[TaskTableField.ConditionXml.ToString()]);
                DeliveryDefId = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.DeliveryDefId.ToString()]);
                FormValidationId = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.FormValidationId.ToString()]);                    
                TaskXmlId = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.TaskXmlId.ToString()]);
                TaskXml = DatabaseUtilities.SetStringValue(dataReader[TaskTableField.TaskXml.ToString()]);
                XSL = dataReader[TaskTableField.XSL.ToString()].ToString();
                SequenceNo = DatabaseUtilities.SetInt32Value(dataReader[TaskTableField.SequenceNo.ToString()]);
                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader[TaskTableField.IsEnabled.ToString()]);
                TaskCreatedDate = Convert.ToDateTime(dataReader["tkCreatedDate"]);
                TaskUpdatedDate = Convert.ToDateTime(dataReader["tkUpdatedDate"]);
                TaskCreatedBy = (int)dataReader["tkCreatedBy"];
                TaskUpdatedByName = DatabaseUtilities.SetStringValue(dataReader["tkUpdatedByName"]);
                ConditionUpdatedDate = Convert.ToDateTime(dataReader["cdUpdatedDate"]);
                ConditionCreatedBy = (int)dataReader["cdCreatedBy"];
                ConditionUpdatedByName = DatabaseUtilities.SetStringValue(dataReader["cdUpdatedByName"]);
                ConditionDescription = DatabaseUtilities.SetStringValue(dataReader["ConditionDescription"]);
                TaskDescription = DatabaseUtilities.SetStringValue(dataReader["TaskDescription"]);
            }          
        }
        #endregion

        #region Properties
        /// <summary>
        /// To get and set ConditionXml
        /// </summary>
        [DataMember]
        public string ConditionXml { get; set; }
                
        /// <summary>
        /// To get and set ConditionXmlId
        /// </summary>
        [DataMember]
        public int ConditionXmlId { get; set; }

        /// <summary>
        /// To get and set DeliveryDefId
        /// </summary>
        [DataMember]
        public Int32 DeliveryDefId { get; set; }
        
        /// <summary>
        /// To get and set FormValidationId
        /// </summary>
        [DataMember]
        public Int32 FormValidationId { get; set; }

        /// <summary>
        /// To get and set Id
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// To get and set Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// To get and set SequenceNo
        /// </summary>
        [DataMember]
        public Int32 SequenceNo { get; set; }

        /// <summary>
        /// To get and set TaskXml
        /// </summary>
        [DataMember]
        public string TaskXml { get; set; }

        /// <summary>
        /// To get and set TaskXmlId
        /// </summary>
        [DataMember]
        public int TaskXmlId { get; set; }

        /// <summary>
        /// To get and set XSL
        /// </summary>
        [DataMember]
        public string XSL { get; set; }

        [DataMember]
        public DateTime TaskCreatedDate { get; set; }

        [DataMember]
        public DateTime TaskUpdatedDate { get; set; }

        [DataMember]
        public int TaskCreatedBy { get; set; }

        [DataMember]
        public string TaskUpdatedByName { get; set; }

        [DataMember]
        public DateTime ConditionCreatedDate { get; set; }

        [DataMember]
        public DateTime ConditionUpdatedDate { get; set; }

        [DataMember]
        public int ConditionCreatedBy { get; set; }

        [DataMember]
        public string ConditionUpdatedByName { get; set; }

        [DataMember]
        public string ConditionDescription { get; set; }

        [DataMember]
        public string TaskDescription { get; set; }
        
        #endregion
    }
}
