using System;
using System.Activities;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class LogInstantPostBlackedOut : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            InstantPostEndpoint ipe = Endpoint.Get(context) as InstantPostEndpoint;
            DeliveryLeadData ld = LeadData.Get(context);

            Console.WriteLine(ld.LeadId + " - Log POST Blacked Out. Delay Delivery To: " + ipe.LivePostUrl);

        }
    }
}
