using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class UpdateLeadStatusIfLastRealtimeDelivery : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public OutArgument<bool> WasLastRealtimeDelivery { get; set; }
        public InArgument<int> LeadRealtimeDeliveryStatus { get; set; }
        public InArgument<DeliveryEndpoint> EndPoint { get; set; }
        public InArgument<int> SchoolValidationResponseStatus { get; set; }
        public InArgument<bool> IsBeta { get; set; }
        public InArgument<int> ReviewedStatusID { get; set; }
               

        protected override void Execute(CodeActivityContext context)
        {
            int leadId = LeadId.Get(context);
            int leadRealtimeDeliveryStatus = LeadRealtimeDeliveryStatus.Get(context);
            bool wasLastRealtimeDelivery = false;
            DeliveryEndpoint endpoint = EndPoint.Get(context);
            bool isTest = false;
            int schoolValidationResponseStatus = SchoolValidationResponseStatus.Get(context);
            int reviewedStatusID = ReviewedStatusID.Get(context);            
           
            //Update Record in Lead Table with RealtimeDeliveryStatus
            if (endpoint != null) isTest = endpoint.IsTest;
            
            IDeliveryEngineDAO deDao = new DeliveryEngineDAO();
            //If the School Validation Response status is not as Duplicate Response Status(450) or Failure Response Status(460)
            int status = schoolValidationResponseStatus != 450
                                 && schoolValidationResponseStatus != 460
                                 && schoolValidationResponseStatus != 480
                                 && schoolValidationResponseStatus != 485
                                 && schoolValidationResponseStatus != 490
                                 && schoolValidationResponseStatus != 475                                
                                  ? leadRealtimeDeliveryStatus
                                  : schoolValidationResponseStatus;
            bool isbeta = IsBeta.Get(context);
            wasLastRealtimeDelivery = deDao.UpdateLeadStatusIfLastRealtimeDelivery(leadId, status, 1, isTest, endpoint.IsPrimary ? true : false, isbeta);

            
            WasLastRealtimeDelivery.Set(context, wasLastRealtimeDelivery);
            ReviewedStatusID.Set(context, reviewedStatusID);
            
        }
    }
}




