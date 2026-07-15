using System;
using System.Activities;
using System.Text.RegularExpressions;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class CreateEmailBody : CodeActivity
    {
        public InArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public OutArgument<string> EmailBody { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            var leadData = LeadData.Get(context);
            var emailBody = string.Empty;

            if (endpoint.IsEmailTemplate)
            {
                Regex rgx;
                emailBody = endpoint.EmailTemplate;

                if (!String.IsNullOrEmpty(emailBody))
                {
                    string rplString;

                    foreach (var item in leadData.TransformedNameValuePairs)
                    {
                        rplString = "<key>" + item.Key + "</key>";
                        emailBody = emailBody.Replace(rplString, item.Value.ToString());
                    }

                    //remove unused replacers
                    rplString = "<key.*?</key>";
                    rgx = new Regex(rplString, RegexOptions.IgnoreCase);
                    emailBody = rgx.Replace(emailBody, "");
                }
            }
            else
            {
                string bodyFooter;
                string bodyHeader;
                string fieldDelimiter;
                string lineDelimiter;
                switch (endpoint.BodyType)
                {
                    case EmailBodyType.HTML:
                        {
                            bodyHeader = IsNullString(endpoint.BodyHeader, "<html><head></head><body><br />");
                            fieldDelimiter = IsNullString(endpoint.FieldDelimiter, ": ");
                            lineDelimiter = IsNullString(endpoint.LineDelimiter, "<br />");
                            bodyFooter = IsNullString(endpoint.BodyFooter, "</body></html>");
                            break;
                        }
                    case EmailBodyType.Text:
                        lineDelimiter = IsNullString(endpoint.LineDelimiter, Environment.NewLine);
                        bodyHeader = IsNullString(endpoint.BodyHeader, lineDelimiter);
                        bodyHeader = bodyHeader + lineDelimiter;
                        fieldDelimiter = IsNullString(endpoint.FieldDelimiter, ": ");
                        bodyFooter = IsNullString(endpoint.BodyFooter, string.Empty);
                        break;
                    default:
                        {
                            bodyHeader = IsNullString(endpoint.BodyHeader,
                                                      "Inquiry Data: " + Environment.NewLine + Environment.NewLine);
                            bodyHeader = bodyHeader + Environment.NewLine;
                            fieldDelimiter = IsNullString(endpoint.FieldDelimiter, ": ");
                            lineDelimiter = IsNullString(endpoint.LineDelimiter, Environment.NewLine);
                            bodyFooter = IsNullString(endpoint.BodyFooter, string.Empty);
                            break;
                        }
                }

                //build postData String
                emailBody += bodyHeader;
                foreach (var item in leadData.TransformedNameValuePairs)
                {
                    emailBody += item.Key + fieldDelimiter + item.Value.ToString() + lineDelimiter;
                }
                emailBody += bodyFooter;
            }
            // Store the request in the OutArgument
            EmailBody.Set(context, emailBody);
        }

        private static string IsNullString(string input, string defaultString)
        {
            return string.IsNullOrWhiteSpace(input) ? defaultString : input;
        }
    }
}
