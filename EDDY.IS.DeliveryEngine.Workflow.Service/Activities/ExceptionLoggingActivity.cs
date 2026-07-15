using System;
using System.Activities;
using System.Diagnostics;
using EDDY.Nexus.Common.ExceptionHandler;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{

    public sealed class ExceptionLoggingActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<Exception> Ex { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            Exception ex = context.GetValue(Ex);

            EventLog.WriteEntry("EDDY", ex.ToString());
            ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.WORKFLOW_SERVICE_POLICY);
        }
    }
}
