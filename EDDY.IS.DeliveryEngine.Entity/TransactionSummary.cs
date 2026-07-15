using System;
using System.Data;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity.ProductProcessing
{
    public class TransactionSummary
    {
        public TransactionSummary()
        {
           
        }

        public TransactionSummary(Int64 transactionSummaryId, string transactionId, string trackingId, Int64 rawPostDataId,
                                  Int32 formValidationId, Int32 formId, Int64 leadId, DateTime startDate,
                                  DateTime finishLeadTableDate, DateTime finishDeliveryDate )
        {
            this.TransactionSummaryId = transactionSummaryId;
            this.TransactionId = transactionId;
            this.TrackingId = trackingId;
            this.RawPostDataId = rawPostDataId;
            this.FormValidationId = formValidationId;
            this.FormId = formId;
            this.LeadId = leadId;
            this.StartDate = startDate;
            this.FinishLeadTableDate = finishLeadTableDate;
            this.FinishDeliveryDate = finishDeliveryDate;
        }

        public TransactionSummary(IDataReader dataReader)  
        {
           
            if (dataReader != null)
            {
                this.TransactionSummaryId = DatabaseUtilities.SetInt64Value(dataReader[TransactionSummaryTableField.TransactionSummaryId.ToString()]);
                this.TransactionId = dataReader[TransactionSummaryTableField.TransactionId.ToString()].ToString();
                this.TrackingId =dataReader[TransactionSummaryTableField.TrackingId.ToString()].ToString();
                this.RawPostDataId = DatabaseUtilities.SetInt64Value(dataReader[TransactionSummaryTableField.RawPostDataId.ToString()]);
                this.FormValidationId = DatabaseUtilities.SetInt32Value(dataReader[TransactionSummaryTableField.FormValidationId.ToString()]);
                this.FormId = DatabaseUtilities.SetInt32Value(dataReader[TransactionSummaryTableField.FormId.ToString()]);
                this.LeadId = DatabaseUtilities.SetInt64Value(dataReader[TransactionSummaryTableField.LeadId.ToString()]);
                this.StartDate = DatabaseUtilities.SetDateTimeValue(dataReader[TransactionSummaryTableField.StartDate.ToString()],false);
                this.FinishLeadTableDate = DatabaseUtilities.SetDateTimeValue(dataReader[TransactionSummaryTableField.FinishLeadTableDate.ToString()],false);
                this.FinishDeliveryDate = DatabaseUtilities.SetDateTimeValue(dataReader[TransactionSummaryTableField.FinishDeliveryDate.ToString()],false);
            }
            
           
        }

        public Int64 TransactionSummaryId { get; set; }

        public string TransactionId { get; set; }

        public string TrackingId { get; set; }

        public Int64 RawPostDataId { get; set; }

        public Int32 FormValidationId { get; set; }

        public Int32 FormId { get; set; }

        public Int64 LeadId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishLeadTableDate { get; set; }

        public DateTime FinishDeliveryDate { get; set; }        

    }

    enum TransactionSummaryTableField
    {
         TransactionSummaryId,
		 TransactionId,
		 TrackingId,
		 RawPostDataId,
		 FormValidationId,
		 FormId,
		 LeadId,
		 StartDate,
         FinishLeadTableDate,
         FinishDeliveryDate
    }
}
