using System;
using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ProductProcessing.Interface;
using EDDY.IS.DeliveryEngine.Entity.ProductProcessing;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.BusinessComponent.Interface.ProductProcessing;
//using EDDY.Nexus.DataAccess.Interface.ProductProcessing;
//using EDDY.Nexus.DataAccess.ProductProcessing;
//using EDDY.Nexus.Entity.ProductProcessing;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ProductProcessing
{
    public class ProductProcessingTransactionManager : IProductProcessingTransactionManager
    {
        private static readonly object _Locker = new object();

        public void InsertTransactionDetail(string transactionId, int activityStepId, string additionalInfo)
        {
            lock (_Locker)
            {
                ProductProcessingTransactionDataService.ProductProcessingTransactionDAO.InsertTransactionDetail(transactionId, activityStepId, additionalInfo);
            }
        }

        public void SaveTransactionSummary(string transactionId, string trackingId, Int64 rawPostDataId,
                                           Int32 formValidationId, Int32 formId, Int64 leadId)
        {
            lock (_Locker)
            {
                ProductProcessingTransactionDataService.ProductProcessingTransactionDAO.SaveTransactionSummary(transactionId, trackingId, rawPostDataId,
                                                        formValidationId, formId, leadId);
            }
        }


        public List<ActivityStep> GetActivitySteps()
        {
            return new List<ActivityStep>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TransactionSummary> GetTransactionSummary()
        {
             return (new ProductProcessingTransactionDAO()).GetTransactionSummary();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetail(string transactionId)
        {
            return (new ProductProcessingTransactionDAO()).GetTransactionDetail(transactionId);
         
        }

    }
}
