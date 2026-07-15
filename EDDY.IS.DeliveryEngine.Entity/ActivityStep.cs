using System;

namespace EDDY.IS.DeliveryEngine.Entity.ProductProcessing
{
    public class ActivityStep
    {
        public ActivityStep()
        {
           
        }

        public ActivityStep(Int32 activityStepId, string activityStepName)
        {
            this.ActivityStepId = activityStepId;
            this.ActivityStepName = activityStepName;
        }

        public Int32 ActivityStepId { get; set; }

        public string ActivityStepName { get; set; }


    }
}
