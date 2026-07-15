using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogFailedPost : CodeActivity
    {
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> PostData { get; set; }
        public InArgument<string> PostResponse { get; set; }
        public InArgument<string> ServerResponseCode { get; set; }
        public OutArgument<bool> logStatus { get; set; }
        public InArgument<int> SchoolValidationResponseStatusID { get; set; }
        public InArgument<int> RejectionReasonID { get; set; }
        public InArgument<int> ReviewedStatusID { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            InstantPostEndpoint endpoint = Endpoint.Get(context) as InstantPostEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string postData = PostData.Get(context);
            string postResponse = PostResponse.Get(context);
            string serverResponse = ServerResponseCode.Get(context);
            string urlPosted = endpoint.IsTest ? endpoint.TestPostUrl : endpoint.LivePostUrl;
            int rejectionReasonId = RejectionReasonID.Get(context);
            int reviewedStatusId = ReviewedStatusID.Get(context);
            int schoolValidationResponseStatusId = SchoolValidationResponseStatusID.Get(context);
            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid();

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Failed Post & Update (original RealtimeDeliveryStatusId, rejectionReasonID if any) in RealtimeDeliveryLog table & (newStatusID) in LEAD table.
            IDeliveryEngineDAO dao = new DeliveryEngineDAO();
            bool logSuccess = dao.LogRealtimeDeliveryComplete(leadData.LeadId, endpoint.DeliveryEndpointId, postData, endpoint.EndpointDetailXML, false, rdq.LastDeliveryAttemptDatetime, rdq.CurrentDeliveryAttempts, serverResponse, postResponse, machineKey, 1, leadData.IsBeta, urlPosted, rejectionReasonId, reviewedStatusId, schoolValidationResponseStatusId);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log POST Failure. Server Response: " + serverResponse);
            
            logStatus.Set(context, logSuccess);
            ReviewedStatusID.Set(context, reviewedStatusId);
        }
    }
}
