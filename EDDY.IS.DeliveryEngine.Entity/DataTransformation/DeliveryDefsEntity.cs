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
    public class DeliveryDefsEntity : CommonEntity, IDeliveryDefsEntity
    {
      #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DeliveryDefsEntity()
        {

        }

        /// <summary>
        /// To initialize the properties with dataRead values
        /// </summary>
        /// <param name="dataReader">Data reader with data</param>
        public DeliveryDefsEntity(IDataReader dataReader) : this()
        {
 
            if (dataReader != null)
            {
                DeliveryDefId = DatabaseUtilities.SetInt32Value(dataReader[(int)DeliveryDefTableField.DeliveryDefId]);
                Description = DatabaseUtilities.SetStringValue(dataReader[(int)DeliveryDefTableField.Description]);
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
        /// To get and set DeliveryDefId
        /// </summary>
        [DataMember]
        public int DeliveryDefId { get; set; }

        /// <summary>
        /// To get and set DeliveryDefDescription
        /// </summary>
        [DataMember]
        public string Description { get; set; }


        #endregion
    }
}
