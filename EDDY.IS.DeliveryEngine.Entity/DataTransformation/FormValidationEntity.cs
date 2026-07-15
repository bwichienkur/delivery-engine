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
    public class FormValidationEntity : CommonEntity, IFormValidationEntity
    {
      #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public FormValidationEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public FormValidationEntity(IDataReader dataReader)  : this()
        {
           
            if (dataReader != null)
            {
                FormValidationId = DatabaseUtilities.SetInt32Value(dataReader[(int)FormValidationTableField.FormValidationId]);
                Description = DatabaseUtilities.SetStringValue(dataReader[(int)FormValidationTableField.Description]);
                //this.IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader[(int)DeliveryDefTableField.IsEnabled]);
                //this.CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)DeliveryDefTableField.CreateDate], false);
                //this.CreatedBy = DatabaseUtilities.SetGuidValue(dataReader[(int)DeliveryDefTableField.CreatedBy]);
                //this.UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader[(int)DeliveryDefTableField.UpdateDate], false);
                //this.UpdatedBy = DatabaseUtilities.SetGuidValue(dataReader[(int)DeliveryDefTableField.UpdatedBy]);
                //this.RowGuid = DatabaseUtilities.SetGuidValue(dataReader[(int)DeliveryDefTableField.RowGuid]);
            }
            
        }
        #endregion

        #region Properties

        /// <summary>
        /// To get and set FormValidationId
        /// </summary>
        [DataMember]
        public int FormValidationId { get; set; }

        /// <summary>
        /// To get and set FormValidationDefDescription
        /// </summary>
        [DataMember]
        public string Description { get; set; }


        #endregion
    }
}
