using System;
using System.Activities;
using System.Collections.Generic;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetLeadRealtimeStatusFromEndpointStatuses : CodeActivity
    {
        public InArgument<ICollection<Int32>> RealtimeEndpointStatuses { get; set; }
        public OutArgument<Int32> LeadRealtimeDeliveryStatus { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }//Added By Bala 12/08/2011
        protected override void Execute(CodeActivityContext context)
        {
            ICollection<Int32> realtimeEndpointStatuses = RealtimeEndpointStatuses.Get(context);

            int leadRealtimeDeliveryStatus = 110; //default to processing

            DeliveryEndpoint endpoint = Endpoint.Get(context);

                foreach (int status in realtimeEndpointStatuses)
                {
                    switch (status)
                    {
                        case 515: // FAILED - WAITING FOR RETRY
                        case 516: // BLACKED OUT - WAITING FOR RETRY
                            {
                                leadRealtimeDeliveryStatus = 120; //PARTIALLY PROCESSED (ONE OR MORE ENDPOINTS CURRENTLY RETRYING)
                                break;
                            }
                        case 610:
                            {
                                leadRealtimeDeliveryStatus = endpoint.IsTest?420:410; //ONE OR MORE ENDPOINTS PERMANENTLY FAILED
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }

                //If we did not have any failures we are still at the default 110
                if (leadRealtimeDeliveryStatus == 110)
                { 
                    //change to success for all
                    leadRealtimeDeliveryStatus = endpoint.IsTest ? 220 : 200;
                }

         

            //store 
            LeadRealtimeDeliveryStatus.Set(context, leadRealtimeDeliveryStatus);
        }

    }
}
