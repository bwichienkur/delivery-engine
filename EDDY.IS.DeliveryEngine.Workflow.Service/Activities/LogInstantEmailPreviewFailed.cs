using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogInstantEmailPreviewFailed : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            InstantEmailEndpoint endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log Email Falied. Send failed To: " + endpoint.LiveEmailTo);

        }
    }
}
