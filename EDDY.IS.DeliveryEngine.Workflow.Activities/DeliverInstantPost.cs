using System;
using System.Activities;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Workflow.Activities.Helper;
using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.DataAccess;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
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

            //Transform post URL into variable URL if it has the proper endpoint details
            CheckForVariablePostUrlSettings(endpoint, leadData);

            //Get Test v Live Post URL
            string postUrl = endpoint.IsTest ? endpoint.TestPostUrl : endpoint.LivePostUrl;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
           
            //attempt post
            try
            {
                #region Code-PostRoman - Updated to handle for the custom http headers setup
                HttpPost httpPost = new HttpPost();
                List<DeliveryPostHTTPHeader> DeliveryPostHTTPHeaderList = (new DeliveryEngineDAO()).GetHTTPHeaders(endpoint.DeliveryEndpointId);
                                
                if (DeliveryPostHTTPHeaderList.Count > 0 || endpoint.PostContentType == PostContentType.JSON)
                    webResponse = httpPost.SendHttpPost(postUrl, postData, endpoint.PostMethod, endpoint.PostContentType, endpoint.SoapAction, DeliveryPostHTTPHeaderList);
                else
                    webResponse = httpPost.SendHttpPost(postUrl, postData, endpoint.PostMethod, endpoint.PostContentType, endpoint.SoapAction);

                //HttpPost httpPost = new HttpPost();
                //webResponse = httpPost.SendHttpPost(postUrl, postData, endpoint.PostMethod, endpoint.PostContentType, endpoint.SoapAction);
                #endregion

                //capture response
                postResponse = webResponse.ResponseMessage;
                responseCode = webResponse.ResponseCode;

            }
            catch (Exception ex)
            {
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.WORKFLOW_SERVICE_POLICY_NO_RETHROW);
                postResponse = "Error posting to " + postUrl + " - " + ex.ToString();
            }

            // Store vals in the OutArgument
            ServerResponse.Set(context, responseCode);
            PostResponse.Set(context, postResponse);
            Endpoint.Set(context, endpoint);//Added By Bala 11/30/2011
        }

        protected void CheckForVariablePostUrlSettings(InstantPostEndpoint endpoint, DeliveryLeadData leadData)
        {
            Dictionary<string, string> endpointDetails = endpoint.EndPointDetailDictionary;
            Dictionary<string, object> leadDataField = leadData.AllFields;
            
            //Check for variable segment flag.
            if (endpointDetails.ContainsKey("VariableURLSegmentPlaceHolder") && endpointDetails.ContainsKey("VariableURLSegmentLeadDataValue"))
            {
                //Get test or live postURL
                string postUrl = endpoint.IsTest ? endpoint.TestPostUrl : endpoint.LivePostUrl;

                //get placeholder value, replacement lead data key, and replacement data value if it exists.
                string placeholder = endpointDetails["VariableURLSegmentPlaceHolder"];
                string replacementValueKey = endpointDetails["VariableURLSegmentLeadDataValue"];
                string replacementValue = leadDataField.ContainsKey(replacementValueKey) ? leadDataField[replacementValueKey].ToString() : string.Empty;

                //If all three of the above are there, we replace the url segment with the intended value.
                if(!String.IsNullOrEmpty(replacementValue))
                {
                    postUrl = postUrl.Replace(placeholder, replacementValue);

                    //Replace test or live url
                    if (endpoint.IsTest)
                        endpoint.TestPostUrl = postUrl;
                    else
                        endpoint.LivePostUrl = postUrl;
                }
            }           
        }
    }
}
