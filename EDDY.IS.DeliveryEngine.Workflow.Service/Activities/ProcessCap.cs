using System;
using System.Activities;
using System.Diagnostics;
//using EDDY.Nexus.BusinessComponent.CoreBusiness;
//using EDDY.Nexus.BusinessComponent.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Workflow.Activities.Cap;
using EDDY.IS.DeliveryEngine.Workflow.Activities.Cap.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{
    public sealed class ProcessCap : CodeActivity
    {
        public InArgument<DeliveryLeadData> inLeadData { get; set; }
        public OutArgument<bool> outSuccess { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DeliveryLeadData leadData = inLeadData.Get(context);
            bool success = false;

            try
            {
                ICapDistributionComponent capDistribution = new CapDistributionComponent();

                //_Success = CapDistribution.UpdateCapValueByCSRId(_leadData);                
                 capDistribution.ProcessCap(leadData);
                 success = true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("EDDY", ex.ToString());
            } 

            //return filePath 
            outSuccess.Set(context, success);

        }
    }
}
