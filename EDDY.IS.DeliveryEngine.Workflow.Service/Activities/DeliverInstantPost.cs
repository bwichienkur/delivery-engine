using System;
using System.Activities;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class DeliverInstantPost : CodeActivity
    {
        public InOutArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<string> PostData { get; set; }
        public OutArgument<string> PostResponse { get; set; }
        public OutArgument<string> ServerResponse { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }//Added By Bala 11/30/2011
        
        
        protected override void Execute(CodeActivityContext context)
        {
            //pull InArgument parameters into vars
            InstantPostEndpoint endpoint = Endpoint.Get(context) as InstantPostEndpoint;
            string postData = PostData.Get(context);
            DeliveryLeadData leadData = LeadData.Get(context);
            //response parameters
            string postResponse = string.Empty;
            string responseCode = string.Empty;
            WebResponse webResponse;
            endpoint.IsTest = endpoint.IsTestModeOverRide(leadData);//Added By Bala 11/30/2011
           
                //Get Test v Live Post URL
                string postUrl;
                if (endpoint.IsTest)
                {
                    postUrl = endpoint.TestPostUrl;
                }
                else
                {
                    postUrl = endpoint.LivePostUrl;
                }

                //attempt post
            try
            {
                HttpPost httpPost = new HttpPost();
                webResponse = httpPost.SendHttpPost(postUrl, postData, endpoint.PostMethod, endpoint.PostContentType, endpoint.SoapAction);

                //capture response
                postResponse = webResponse.ResponseMessage;
                responseCode = webResponse.ResponseCode;
 
            }
            catch (Exception ex)
            {
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.WORKFLOW_SERVICE_POLICY_NO_RETHROW);
                postResponse = "Error "+ex.ToString();
            }
                               
           

            // Store vals in the OutArgument
            ServerResponse.Set(context, responseCode);
            PostResponse.Set(context, postResponse);
            Endpoint.Set(context, endpoint);//Added By Bala 11/30/2011
        }
    }
}
