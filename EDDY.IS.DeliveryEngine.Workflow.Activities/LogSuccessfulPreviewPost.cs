using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public sealed class LogSuccessfulPreviewPost : CodeActivity
    {
        
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> PostData { get; set; }
        public InArgument<string> PostResponse { get; set; }
        public InArgument<string> ServerResponseCode { get; set; }
        public OutArgument<bool> LogSuccessfulPostStatus { get; set; }
        public InOutArgument<Dictionary<string,string>> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {

            InstantPostEndpoint endpoint = Endpoint.Get(context) as InstantPostEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string postData = PostData.Get(context);
            string postResponse = PostResponse.Get(context);
            string serverResponse = ServerResponseCode.Get(context);
            Dictionary<string, string> result = Result.Get(context);

            if (result == null) result = new Dictionary<string, string> {};
            if (!result.ContainsKey(endpoint.DeliveryEndpointId.ToString() + ";" + endpoint.DeliveryDefinitionId.ToString() + ";" + endpoint.EndpointName + ";" + " Post"))
                result.Add(endpoint.DeliveryEndpointId.ToString() + ";" + endpoint.DeliveryDefinitionId.ToString() + ";" + endpoint.EndpointName + ";" + " Post", postData); 
            
            //change this to pull userguid from session
            Guid userGuid = Guid.NewGuid();

            //Get Machine Key
            Guid machineKey = new Guid(System.Configuration.ConfigurationManager.AppSettings["MachineKey"]);

            //Log Failed Post
            bool logSuccess = DeliveryEngineDataService.DeliveryEngineDAO.LogPreviewDeliveryComplete(leadData.LeadId, endpoint.DeliveryDefinitionId, postData, endpoint.EndpointDetailXML, true, DateTime.Now , 0, serverResponse, postResponse, machineKey, 1);

            Console.WriteLine(leadData.LeadId + "-ep:" + endpoint.DeliveryEndpointId.ToString() + " - Log POST Success. Server Response: " + serverResponse);
            LogSuccessfulPostStatus.Set(context, logSuccess);

            Result.Set(context, result);
        }
    }
}
