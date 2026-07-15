using System;
using System.Activities;
//using EDDY.Nexus.BusinessComponent.Interface.ProductProcessing;
//using EDDY.Nexus.BusinessComponent.ProductProcessing;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ProductProcessing;
using EDDY.IS.DeliveryEngine.Workflow.Activities.ProductProcessing.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{
    public sealed class InsertTransactionDetail : CodeActivity
    {
        public InArgument<String> inTransactionId { get; set; }
        public InArgument<Int32> inActivityStepId { get; set; }
        public InArgument<String> inAdditionalInfo { get; set; }        

        protected override void Execute(CodeActivityContext context)
        {                     
            string transactionId = inTransactionId.Get(context);
            int activityStepId = inActivityStepId.Get(context);
            string additionalInfo = inAdditionalInfo.Get(context);

            IProductProcessingTransactionManager transactionManager = new ProductProcessingTransactionManager();
            transactionManager.InsertTransactionDetail(transactionId, activityStepId, additionalInfo);
            

        }
    }
}


