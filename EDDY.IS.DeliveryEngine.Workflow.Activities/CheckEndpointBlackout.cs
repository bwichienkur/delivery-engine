using System;
using System.Activities;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class CheckEndpointBlackout : CodeActivity
    {
        public InArgument<DeliveryEndpoint> DeliveryEndpoint { get; set; }
        public OutArgument<bool> IsBlackedOut { get; set; }
        public OutArgument<DateTime> NextDeliveryAttemptDateTime { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            //Interrogate the DeliveryEndpoint to see if delivery is currently blacked out

            DeliveryEndpoint endpoint = DeliveryEndpoint.Get(context);
            bool endpointIsBlackedOut = false; //default
            DateTime nextDeliveryAttemptDateTime = DateTime.Now.ToUniversalTime(); //default

            //Take a snapshot of current time
            DateTime dtNow = DateTime.Now.ToUniversalTime();

            //For each individual blackout routine return
            BlackoutToken blackoutToken;

            //Get list of current blackout periods for this endpoint
            if (endpoint.BlackoutPeriods != null)
            {
                //loop through blackout periods
                foreach (DeliveryBlackoutPeriod dbp in endpoint.BlackoutPeriods)
                {
                    //Get Blackout info for dtNow
                    blackoutToken = dbp.CheckDateBlackedOut(dtNow);

                    //Update blacked out (logical OR will return TRUE after loop if any blackout is true)
                    endpointIsBlackedOut = endpointIsBlackedOut || blackoutToken.BlackoutCurrentlyEffective;

                    if (blackoutToken.BlackoutCurrentlyEffective)
                    { 
                        //If this blackout ends AFTER the current nextDeliveryAttemptDateTime, make it the new nextDeliveryAttemptDateTime
                        if (blackoutToken.BlackoutEndDateTime > nextDeliveryAttemptDateTime)
                        {
                            nextDeliveryAttemptDateTime = blackoutToken.BlackoutEndDateTime;
                        }
                    }

                }
            }

            // Output
            IsBlackedOut.Set(context, endpointIsBlackedOut);
            NextDeliveryAttemptDateTime.Set(context, nextDeliveryAttemptDateTime);
        }


    }
}
