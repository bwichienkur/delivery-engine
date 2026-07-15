using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryPI:CommonEntity
    {
        [DataMember]
        public int DeliveryEndpointId { get; set; }
        [DataMember]
        public int DelDefID { get; set; }
        [DataMember]
        public string EndPointName { get; set; }
        [DataMember]
        public int DeliveryTypeId { get; set; }
        [DataMember]
        public string DeliveryName { get; set; }
        [DataMember]
        public string EndpointDetailXML { get; set; }
        [DataMember]
        public string BatchFileXSL { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public DateTime EffectiveDate{ get; set; }
        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public int DefaultPaidTemplateProductId { get; set; }
       
        #region Constructor
        public DeliveryPI()
        {

        }

        public DeliveryPI(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                DeliveryTypeId = DatabaseUtilities.SetInt32Value(dataReader["DataTypeShortName"]);
                DeliveryName = DatabaseUtilities.SetStringValue(dataReader["DeliveryName"]);
                EndpointDetailXML = DatabaseUtilities.SetStringValue(dataReader["EndpointDetailXML"]);
                
                UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"],true);
                UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["UpdatedDate"], true);
                EffectiveDate = DatabaseUtilities.SetDateTimeValue(dataReader["EffectiveDate"], true);
                ExpirationDate = DatabaseUtilities.SetDateTimeValue(dataReader["ExpirationDate"], true);
                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]);
                DefaultPaidTemplateProductId = DatabaseUtilities.SetInt32Value(dataReader["DefaultPaidTemplateProductId"]);
            }
        }

        //public DeliveryPI ReturnPI(IDataReader dataReader)
        //{

        //    if (dataReader != null)
        //    {
        //        DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
        //        DeliveryTypeId = DatabaseUtilities.SetInt32Value(dataReader["DataTypeShortName"]);
        //        DeliveryName = DatabaseUtilities.SetStringValue(dataReader["DeliveryName"]);
        //        EndpointDetailXML = DatabaseUtilities.SetStringValue(dataReader["EndpointDetailXML"]);
        //        UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
        //        CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"], true);
        //        UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["UpdatedDate"], true);
        //        EffectiveDate = DatabaseUtilities.SetDateTimeValue(dataReader["EffectiveDate"], true);
        //        ExpirationDate = DatabaseUtilities.SetDateTimeValue(dataReader["ExpirationDate"], true);
        //        IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]);
        //    }
        //    return this;
        //}
       
        #endregion

    }
}
