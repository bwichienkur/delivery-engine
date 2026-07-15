using System;
using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.DeliveryEngine;
using System.Text;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class FinalizeLeadDelivery : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> LeadRealtimeDeliveryStatus { get; set; }
        public OutArgument<bool> RealtimeDeliveryStatusUpdated { get; set; }
        public InArgument<int> SchoolValidationResponseStatus { get; set;}
        protected override void Execute(CodeActivityContext context)
        {
            try
            {              
                int leadId = LeadId.Get(context);
                int leadRealtimeDeliveryStatus = LeadRealtimeDeliveryStatus.Get(context);
                int schoolValidationResponseStatus = SchoolValidationResponseStatus.Get(context);
                int status = 0;

                //Update Record in Lead Table with RealtimeDeliveryStatus
                //If the School Validation Response status is not as Duplicate Response Status(450) or Failure Response Status(460)  
                if (leadRealtimeDeliveryStatus != 240 && leadRealtimeDeliveryStatus != 260)
                {
                    status = schoolValidationResponseStatus != 450
                            && schoolValidationResponseStatus != 460
                            && schoolValidationResponseStatus != 480
                            && schoolValidationResponseStatus != 485
                            && schoolValidationResponseStatus != 490
                             && schoolValidationResponseStatus != 475
                             ? leadRealtimeDeliveryStatus
                             : schoolValidationResponseStatus;
                }
                else { 
                    status =  leadRealtimeDeliveryStatus;
                }

                bool result = DeliveryEngineDataService.DeliveryEngineDAO.FinalizeLeadDelivery(leadId, status, 1);
                
                RealtimeDeliveryStatusUpdated.Set(context, result);
                
            }
            catch (Exception)
            {
                //do nothing
            }
            
        }


        private static bool LogForLeadViewerHistory(int leadId, int originalDeliveryStatusID, int ReviewedDeliveryStatusID)
        {
            var sbOriginal = new StringBuilder();
            sbOriginal.Append("<Lead LeadId='" + leadId + "'><Fields>");
            sbOriginal.Append("<Field FieldName='RealTimeDeliveryStatusId' Value='" + originalDeliveryStatusID + "' />");
            sbOriginal.Append("</Fields></Lead>");

            var sbModified = new StringBuilder();
            sbModified.Append("<Lead LeadId='" + leadId + "'><Fields>");
            sbModified.Append("<Field FieldName='RealTimeDeliveryStatusId' Value='" + ReviewedDeliveryStatusID + "' />");
            sbModified.Append("</Fields></Lead>");

            //Log Record in LeadViewerHistory Table so that the reviewed status becomes available in LeadViewer.
            bool result = DeliveryEngineDataService.DeliveryEngineDAO.LogForLeadViewerHistory(leadId, sbOriginal.ToString(), sbModified.ToString());

            return result;
        }

    }
}
