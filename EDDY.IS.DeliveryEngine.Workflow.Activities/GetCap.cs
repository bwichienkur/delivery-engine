using System;
using System.Activities;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class GetCap : CodeActivity
    {
        public InArgument<Int32> LeadId { get; set; }
        public OutArgument<bool> Capped { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            //TEMP: Fake a call to the DB for the correct Cap Info
            bool capped;
            int leadId = LeadId.Get(context);

            capped = false;

            // Store the request in the OutArgument
            Capped.Set(context, capped);
        }
    }
}
