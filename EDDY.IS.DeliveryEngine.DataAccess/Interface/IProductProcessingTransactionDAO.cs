using System;
//using EDDY.Nexus.DataAccess.Interface.Common;
using EDDY.IS.DeliveryEngine.DataAccess.Interface.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface IProductProcessingTransactionDAO : IBaseDataSource
    {
        # region Public Methods
        bool InsertTransactionDetail(string transactionId, int activityStepId, string additionalInfo);
        bool SaveTransactionSummary(string transactionId, string trackingId, Int64 rawPostDataId,
                                           Int32 formValidationId, Int32 formId, Int64 leadId);


        # endregion
    }

}
