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
    public class XsltXmlEntity : CommonEntity, IDTXsltXmlEntity
    {
        #region Constants

        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public XsltXmlEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public XsltXmlEntity(IDataReader dataReader)
            : this()
        {

            if (dataReader != null)
            {
                TaskID = DatabaseUtilities.SetInt32Value(dataReader["TaskID"]);
                TaskXMLID = DatabaseUtilities.SetInt32Value(dataReader["TaskXMLID"]);
                TaskXML = DatabaseUtilities.SetStringValue(dataReader["TaskXML"]);
                TaskDesc = DatabaseUtilities.SetStringValue(dataReader["TaskDescription"]);
                CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader["CreatedBy"]);
                UpdatedDate = Convert.ToDateTime(dataReader["UpdatedDate"]);
                UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
                XSLT = DatabaseUtilities.SetStringValue(dataReader["XSL"]);
            }

        }
        #endregion
        #region Properties
        [DataMember]
        public int TaskID { get; set; }
        [DataMember]
        public int TaskXMLID { get; set; }
        [DataMember]
        public string TaskXML { get; set; }
        [DataMember]
        public string TaskDesc { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public string XSLT { get; set; }

        #endregion
    }
}
