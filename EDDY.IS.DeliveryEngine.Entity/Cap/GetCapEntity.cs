using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity.Cap
{
     [DataContract]
    public class GetCapEntity : CommonEntity
    {

        #region constructors

        /// <summary>
        /// constructor for ClientEntity
        /// </summary>

         public GetCapEntity(IDataReader dataReader)
        {
            if (dataReader != null)
            {
                this.StartDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)DatabaseTableField.StartDate],true);
                this.EndDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)DatabaseTableField.EndDate],true);

                this.CapValue = DatabaseUtilities.SetInt32Value(dataReader[(int)DatabaseTableField.CapValue]);
                this.CapCount = DatabaseUtilities.SetInt32Value(dataReader[(int)DatabaseTableField.CapCount]);

                this.Status = DatabaseUtilities.SetBooleanValue(dataReader[(int)DatabaseTableField.IsEnabled]);
                
                this.School = DatabaseUtilities.SetStringValue(dataReader[(int)DatabaseTableField.SchoolName]);
                this.Client = DatabaseUtilities.SetStringValue(dataReader[(int)DatabaseTableField.ClientName]);
                this.CapLevel = DatabaseUtilities.SetStringValue(dataReader[(int)DatabaseTableField.CapLevel]);

            }
        }

        #endregion

        #region Data Members

         [DataMember]
         public string Client { get; set; }
        [DataMember]
        public string School { get; set; }
        [DataMember]
        public string CapLevel { get; set; }
        [DataMember]
        public int CapValue { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public int CapCount { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }

       
        #endregion

        #region enums

        public enum DatabaseTableField
        {
            StartDate,
            EndDate,
            CapValue,
            CapCount,
            IsEnabled,
            SchoolName,
            ClientName,
            CapLevel

        }

        #endregion
    }
}
