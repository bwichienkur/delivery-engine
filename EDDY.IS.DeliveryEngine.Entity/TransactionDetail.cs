using System;
using System.Data;
using EDDY.Nexus.Common.Utilities;

namespace EDDY.IS.DeliveryEngine.Entity.ProductProcessing
{
    public class TransactionDetail
    {
        public TransactionDetail()
        {
           
        }

        public TransactionDetail(Int64 transactionDetailId, string transactionId, int activityStepId, 
                                 string additionalInfo, DateTime createDate)
        {
            this.TransactionDetailId = transactionDetailId;
            this.TransactionId = transactionId;
            this.ActivityStepId = activityStepId;
            this.AdditionalInfo = additionalInfo;
            this.CreateDate = createDate;        
        }


        public TransactionDetail(IDataReader dataReader): this()
        {
           
            if (dataReader != null)
            {
                this.TransactionDetailId = DatabaseUtilities.SetInt64Value(dataReader[TransactionDetailTableField.TransactionDetailId.ToString()]);
                this.TransactionId =dataReader[TransactionDetailTableField.TransactionId.ToString()].ToString();
                this.ActivityStepId = DatabaseUtilities.SetInt32Value(dataReader[TransactionDetailTableField.ActivityStepId.ToString()]);
                this.AdditionalInfo = DatabaseUtilities.SetStringValue(dataReader[TransactionDetailTableField.AdditionalInfo.ToString()]);
                this.CreateDate = DatabaseUtilities.SetDateTimeValue(dataReader[TransactionDetailTableField.CreateDate.ToString()],false);
                this.ActivityStepName = DatabaseUtilities.SetStringValue(dataReader[TransactionDetailTableField.ActivityStepName.ToString()]);
            }
            
           
        }
        public Int64 TransactionDetailId { get; set; }

        public string TransactionId { get; set; }

        public int ActivityStepId { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTime CreateDate { get; set; }

        public string ActivityStepName { get; set; }
        
    }

    enum TransactionDetailTableField
    {

         TransactionDetailId,
		 TransactionId,
		 ActivityStepId,
		 AdditionalInfo,
		 CreateDate,
		 ActivityStepName
    }
}
