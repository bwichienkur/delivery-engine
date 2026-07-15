using System;
using System.Activities;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogInstantEmailFailed : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<RealtimeDeliveryQueueItem> RDQ { get; set; }
        public InArgument<string> EmailBody { get; set; }
        

        protected override void Execute(CodeActivityContext context)
        {
            InstantEmailEndpoint endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            RealtimeDeliveryQueueItem rdq = RDQ.Get(context);
            string emailBody = EmailBody.Get(context);
            string emailAddressesDelivered = endpoint.IsTest ? endpoint.TestEmailTo : endpoint.LiveEmailTo;            
            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Email Failed
            IDeliveryEngineDAO dao = new DeliveryEngineDAO();
            bool logSuccess = dao.LogRealtimeDeliveryComplete(leadData.LeadId, endpoint.DeliveryEndpointId, emailBody, endpoint.EndpointDetailXML, true, rdq.LastDeliveryAttemptDatetime, rdq.CurrentDeliveryAttempts, null, null, machineKey, 1, leadData.IsBeta, emailAddressesDelivered, 0, 0, 555);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log Email Falied. Send failed To: " + endpoint.LiveEmailTo);

        }
    }
}
