using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class PreviewError:CodeActivity 
    {
        public InOutArgument<Dictionary<string, string>> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Dictionary<string, string> result = Result.Get(context);
            if (result == null) result = new Dictionary<string, string> {};
            result.Add("Error","Delivery definition not found");
            Result.Set(context, result);
        }
    }
}
