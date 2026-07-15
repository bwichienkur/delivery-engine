using System;
using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryBlackoutItem : CommonEntity
    {
        [DataMember]
        public int DeliveryBlackoutId { get; set; }
        [DataMember]
        public int DeliveryEndpointId { get; set; }
        [DataMember]
        public int DeliveryBlackoutType { get; set; }
        [DataMember]
        public DateTime StartDateTime { get; set; }
        [DataMember]
        public DateTime EndDateTime { get; set; }
        [DataMember]
        public DateTime DailyEndDateTime { get; set; }
        [DataMember]
        public int UtcHourOffset { get; set; }
        [DataMember]
        public string Frequency { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public string BlackoutPeriodName { get; set; }
        [DataMember]
        public string RecurringBlackoutXML { get; set; }
        #region Constructor
        public DeliveryBlackoutItem()
        {

        }

        public DeliveryBlackoutItem(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                DeliveryBlackoutId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryBlackoutId"]);
                DeliveryEndpointId = DatabaseUtilities.SetInt32Value(dataReader["DeliveryEndpointId"]);
                DeliveryBlackoutType = DatabaseUtilities.SetInt32Value(dataReader["DeliveryBlackoutType"]);
                StartDateTime = DatabaseUtilities.SetDateTimeValue(dataReader["StartDateTime"], false);
                EndDateTime = DatabaseUtilities.SetDateTimeValue(dataReader["EndDateTime"], false);
                UtcHourOffset = DatabaseUtilities.SetInt32Value(dataReader["UtcHourOffset"]);
                Frequency = DatabaseUtilities.SetStringValue(dataReader["Frequency"]);
                BlackoutPeriodName = DatabaseUtilities.SetStringValue(dataReader["BlackoutPeriodName"]);
                DailyEndDateTime = DeliveryBlackoutType > 1 ? Convert.ToDateTime(string.Empty + dataReader["DailyEndDateTime"].ToString()) : EndDateTime;

                IsEnabled = DatabaseUtilities.SetBooleanValue(dataReader["IsEnabled"]);
                CreatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["CreatedDate"], false);
                UtcHourOffset = DatabaseUtilities.SetInt32Value(dataReader["UtcHourOffset"]);
                CreatedBy = DatabaseUtilities.SetInt32Value(dataReader["CreatedBy"]);
                UpdatedBy = DatabaseUtilities.SetInt32Value(dataReader["UpdatedBy"]);
                UpdatedDate = DatabaseUtilities.SetDateTimeValue(dataReader["UpdatedDate"], false);
                //UpdatedByName = DatabaseUtilities.SetStringValue(dataReader["UpdatedByName"]);
            }
        }


        #endregion
    }
}
