using System;
using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Entity.ProductProcessing;
//using EDDY.Nexus.Entity.ProductProcessing;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ProductProcessing.Interface
{
    public interface IProductProcessingTransactionManager
    {
        #region Methods
        void InsertTransactionDetail(string transactionId, int activityStepId, string additionalInfo);
        void SaveTransactionSummary(string transactionId, string trackingId, Int64 rawPostDataId,
                                           Int32 formValidationId, Int32 formId, Int64 leadId);
        List<ActivityStep> GetActivitySteps();
        List<TransactionSummary> GetTransactionSummary();
        List<TransactionDetail> GetTransactionDetail(string transactionId);

        #endregion
    }
}
