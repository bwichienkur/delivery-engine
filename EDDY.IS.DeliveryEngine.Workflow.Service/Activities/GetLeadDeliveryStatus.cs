using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class GetLeadDeliveryStatus : CodeActivity
    {
        public InArgument<int> LeadId { get; set; }
        public InArgument<int> LeadRealtimeDeliveryStatus { get; set; }
        //public OutArgument<bool> RealtimeDeliveryStatusUpdated { get; set; }
        public InArgument<int> SchoolValidationResponseStatus { get; set; }
        public OutArgument<int> Status { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public OutArgument<int> LeadStatus { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                var status = 0;
                var leadData = LeadData.Get(context);
                int leadId = LeadId.Get(context);
                int leadRealtimeDeliveryStatus = LeadRealtimeDeliveryStatus.Get(context);
                int schoolValidationResponseStatus = SchoolValidationResponseStatus.Get(context);
                //Update Record in Lead Table with RealtimeDeliveryStatus
                IDeliveryEngineDAO deDao = new DeliveryEngineDAO();
                if (leadData != null)
                {
                    //If the School Validation Response status is not as Duplicate Response Status(450) or Failure Response Status(460)
                    status = schoolValidationResponseStatus != 450
                            && schoolValidationResponseStatus != 460
                            && schoolValidationResponseStatus != 480
                            && schoolValidationResponseStatus != 485
                            && schoolValidationResponseStatus != 490
                             && schoolValidationResponseStatus != 475
                             ? leadRealtimeDeliveryStatus
                             : schoolValidationResponseStatus;

                    bool result = deDao.FinalizeLeadDelivery(leadId, status, 1);
                    LeadStatus.Set(context, status);
                    status = deDao.GetCountAgainstCapStatus(status) ? 1 : 0;
                    //var noOfReturnedRecords = deDao.RemoveRePostData(leadId);
                    //RealtimeDeliveryStatusUpdated.Set(context, result);
                    Status.Set(context, status);
                }
                else
                {
                    LeadStatus.Set(context, -1);
                    Status.Set(context, -1);
                }
            }
            catch (Exception)
            {
                //do nothing
            }

        }
    }
}
