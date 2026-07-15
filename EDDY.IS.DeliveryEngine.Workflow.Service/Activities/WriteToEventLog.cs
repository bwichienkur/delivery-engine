using System.Activities;
using System.Diagnostics;

namespace EDDY.IS.DeliveryEngine.Workflow.Service.Activities
{

    public sealed class WriteToEventLog : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> LogEntry { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string logEntry = context.GetValue(LogEntry);

            EventLog.WriteEntry("EDDY", logEntry);
        }
    }
}
