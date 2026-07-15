using System;
using System.Activities;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class LogInstantEmailBlackedOut : CodeActivity
    {
        public InArgument<InstantEmailEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            InstantEmailEndpoint iee = Endpoint.Get(context);
            DeliveryLeadData ld = LeadData.Get(context);

            Console.WriteLine(ld.LeadId + " - Log Email Blacked Out. Delay Delivery To: " + iee.LiveEmailTo);

        }
    }
}
