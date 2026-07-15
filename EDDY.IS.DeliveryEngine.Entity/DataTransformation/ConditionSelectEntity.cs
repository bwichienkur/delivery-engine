using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.Interface.DataTransformation;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{
    public class ConditionSelectEntity : CommonEntity, IDTConditionSelectEntity
    {
         #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConditionSelectEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public ConditionSelectEntity(IDataReader dataReader) : this()
        {
           
            if (dataReader != null)
            {
                TaskID = DatabaseUtilities.SetInt32Value(dataReader["TaskID"]);
                ConditionXMLID = DatabaseUtilities.SetInt32Value(dataReader["ConditionXMLID"]);
                ConditionXML = DatabaseUtilities.SetStringValue(dataReader["ConditionXML"]);
                ConditionDesc = DatabaseUtilities.SetStringValue(dataReader["ConditionDescription"]);
                CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader["CreatedBy"]);
                UpdatedDate = Convert.ToDateTime(dataReader["UpdatedDate"]);
                UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
                
            }
            
        }
        #endregion
        #region Properties
        [DataMember]
        public int TaskID { get; set; }
        [DataMember]
        public int ConditionXMLID { get; set; }
        [DataMember]
        public string ConditionXML { get; set; }
        [DataMember]
        public string ConditionDesc { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        #endregion
    }
}
