using System;
using System.Activities;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class LogInstantEmailSuccessful : CodeActivity
    {
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> EmailBody { get; set; }
        public OutArgument<bool> LogInstantEmailStatus { get; set; }
       
        protected override void Execute(CodeActivityContext context)
        {
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            InstantEmailEndpoint endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string emailBody = EmailBody.Get(context);
            string emailAddressesDelivered = endpoint.IsTest ? endpoint.TestEmailTo : endpoint.LiveEmailTo;            
            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid();

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Email successful
            bool logSuccess = DeliveryEngineDataService.DeliveryEngineDAO.LogRealtimeDeliveryComplete(leadData.LeadId, endpoint.DeliveryEndpointId, emailBody, endpoint.EndpointDetailXML, true, rdq.LastDeliveryAttemptDatetime, rdq.CurrentDeliveryAttempts, null, null, machineKey, 1, leadData.IsBeta, emailAddressesDelivered, 0, 0, 200);

            LogInstantEmailStatus.Set(context, logSuccess);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log Email Sent. Sent To: " + endpoint.LiveEmailTo);

        }
    }
}
