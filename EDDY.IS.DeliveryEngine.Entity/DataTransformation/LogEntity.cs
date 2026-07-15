using System;
using System.Data;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.Interface.DataTransformation;


namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation
{


    public class LogEntity : CommonEntity, ILogEntity
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public LogEntity(IDataReader dataReader)
            : this()
        {
            if (dataReader != null)
            {
                Id = DatabaseUtilities.SetInt32Value(dataReader[(int)LogTableField.Id]);
                LeadId = DatabaseUtilities.SetInt32Value(dataReader[(int)LogTableField.LeadId]);
                RowLeadId = DatabaseUtilities.SetInt32Value(dataReader[(int)LogTableField.RowLeadId]);
                Message = dataReader[(int)LogTableField.LogMessage].ToString();
                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader[(int)TaskTableField.IsEnabled]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)TaskTableField.CreateDate], false);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)TaskTableField.CreatedBy]);
                UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)TaskTableField.UpdateDate], false);
                UpdatedBy = DatabaseUtilities.SetInt32Value(dataReader[(int)TaskTableField.UpdatedBy]);
                RowGuid = DatabaseUtilities.SetGuidValue(dataReader[(int)TaskTableField.RowGuid]);
            }
            
        }
        #endregion

        public Int32 Id { get; set; }
        public int? LeadId { get; set; }
        public int? RowLeadId { get; set; }
        public string Message { get; set; }
    }
}
