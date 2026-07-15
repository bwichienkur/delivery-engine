using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed  class LogInstantEmailPreviewSuccessful:CodeActivity
    {
        
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> EmailBody { get; set; }
        public OutArgument<bool> LogInstantEmailStatus { get; set; }
        public InOutArgument<Dictionary<string, string>> Result { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            InstantEmailEndpoint endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string emailBody = EmailBody.Get(context);

            Dictionary<string, string> result = Result.Get(context);
            if (result == null) result = new Dictionary<string, string> { };

            if (!result.ContainsKey(endpoint.DeliveryEndpointId.ToString() + ";" + endpoint.DeliveryDefinitionId.ToString() + ";" + endpoint.EndpointName + ";" + " Email"))
                result.Add(endpoint.DeliveryEndpointId.ToString()+";" +endpoint.DeliveryDefinitionId.ToString()+";"+endpoint.EndpointName+";"+" Email", emailBody);
            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid();

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Failed Post
            IDeliveryEngineDAO dao = new DeliveryEngineDAO();
            bool logSuccess = dao.LogPreviewDeliveryComplete(leadData.LeadId,endpoint.DeliveryDefinitionId, emailBody, endpoint.EndpointDetailXML, true, DateTime.Now, 0, null, null, machineKey, 1);

            LogInstantEmailStatus.Set(context, logSuccess);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log Email Sent. Sent To: " + endpoint.LiveEmailTo);
            Result.Set(context,result);
        }
    }
}
