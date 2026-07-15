using System;
using System.Activities;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class AdjustLeadRealtimeStatus : CodeActivity
    {
        public InOutArgument<Int32> LeadRealtimeDeliveryStatus { get; set; }
        public InArgument<Int32> CurrentRealtimeEndpointStatus { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }//Added By Bala 12/09/2011        
        public InArgument<int> ReviewedStatusID { get; set; }        

        protected override void Execute(CodeActivityContext context)
        {
            Int32 leadRealtimeDeliveryStatus = LeadRealtimeDeliveryStatus.Get(context);
            Int32 currentRealtimeEndpointStatus = CurrentRealtimeEndpointStatus.Get(context);
            DeliveryEndpoint endpoint = Endpoint.Get(context);
            int reviewedStatusID = ReviewedStatusID.Get(context);            

            if (endpoint.IsPrimary)//Set the Status only if the End Point is Primary
            {
                switch (currentRealtimeEndpointStatus)
                {
                    case 515: // FAILED - WAITING FOR RETRY
                    case 516: // BLACKED OUT - WAITING FOR RETRY
                        {
                            leadRealtimeDeliveryStatus = 120;
                                //PARTIALLY PROCESSED (ONE OR MORE ENDPOINTS CURRENTLY RETRYING)
                            break;
                        }
                    case 610:
                        {
                            leadRealtimeDeliveryStatus = endpoint.IsTest ? 420 : 410;
                                //ONE OR MORE ENDPOINTS PERMANENTLY FAILED
                            break;
                        }
                    case 600: //POST WAS SUCCESSFUL... AND REALTIMEDELIVERYSTATUS WAS REVIEWED AND CHANGED AS PART OF THE AUTO REVIEW PROCESS.
                        {
                            if (reviewedStatusID > 0)
                                leadRealtimeDeliveryStatus = reviewedStatusID;
                            break;
                        }                        
                    default:
                        {
                            break;
                        }
                }

                //If we did not have any failures we are still at the default 110
                if (leadRealtimeDeliveryStatus == 110)
                {
                    //change to success for all
                    //leadRealtimeDeliveryStatus = endpoint.IsTest ? 220 : 200;
                    if (endpoint.IsTest)
                        leadRealtimeDeliveryStatus = endpoint.IsScrub ? 301 : 220; //Internal and test delivered or test deliverd
                    else if (endpoint.IsScrub)
                        leadRealtimeDeliveryStatus =  321; //Internal Deliverd
                    else
                        leadRealtimeDeliveryStatus = 200;
                }

            }


            //store 
            LeadRealtimeDeliveryStatus.Set(context, leadRealtimeDeliveryStatus);
            ReviewedStatusID.Set(context, reviewedStatusID);
          
        }

    }
}

