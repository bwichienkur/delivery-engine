using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class LogFailedPostPreview : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> PostData { get; set; }
        public InArgument<string> PostResponse { get; set; }
        public InArgument<string> ServerResponseCode { get; set; }
        public OutArgument<bool> logStatus { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            InstantPostEndpoint endpoint = Endpoint.Get(context) as InstantPostEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string postData = PostData.Get(context);
            string postResponse = PostResponse.Get(context);
            string serverResponse = ServerResponseCode.Get(context);

            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid();

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Failed Post
            bool logSuccess = DeliveryEngineDataService.DeliveryEngineDAO.LogPreviewDeliveryComplete(leadData.LeadId, endpoint.DeliveryDefinitionId, postData, endpoint.EndpointDetailXML, false, DateTime.Now, 0, serverResponse, postResponse, machineKey, 1);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log POST Failure. Server Response: " + serverResponse);
            logStatus.Set(context, logSuccess);

        }
    }
}
