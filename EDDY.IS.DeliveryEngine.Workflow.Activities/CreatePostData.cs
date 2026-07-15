using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class CreatePostData : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public OutArgument<string> PostData { get; set; }

        // private bool postSuccess;

        protected override void Execute(CodeActivityContext context)
        {
            var endpoint = Endpoint.Get(context) as InstantPostEndpoint;
            var leadData = LeadData.Get(context);
            var postString = string.Empty;


            if (endpoint != null)
            {
                if (endpoint.PostContentType == PostContentType.Form)
                {
                    postString = CreateFormPostString(leadData);
                }
                else if (endpoint.PostContentType == PostContentType.XML)
                {
                    postString = leadData.TransformedNameValuePairs.Count == 1
                                     ? leadData.TransformedNameValuePairs.Values.First().ToString()
                                     : CreateXmlPostString(leadData.TransformedNameValuePairs);
                }
                else if (endpoint.PostContentType == PostContentType.JSON)
                {
                    postString = CreateJSONPostString(leadData);
                }
            }
            // Store the request in the OutArgument
            PostData.Set(context, postString);
        }

        private static string CreateFormPostString(DeliveryLeadData leadData)
        {
            var postString = string.Empty;
            foreach (var item in leadData.TransformedNameValuePairs)
            {
                if (postString != string.Empty) postString += "&";
                postString += item.Key + "=" + item.Value;
            }

            return postString;
        }


        private static string CreateXmlPostString(Dictionary<string, object> rawData)
        {
            var sb = new StringBuilder();
            sb.Append("<xmlData><fields>");
            foreach (var item in rawData)
            {
                sb.Append("<field name='" + item.Key + "' value='" + item.Value + "' />");
            }
            sb.Append("</fields></xmlData>");
            return sb.ToString();
        }

        private static string CreateJSONPostString(DeliveryLeadData leadData)
        {
            var postString = string.Empty;

            foreach (var item in leadData.TransformedNameValuePairs)
            {
                postString += item.Value;
            }

            return postString;
        }
    }
}
