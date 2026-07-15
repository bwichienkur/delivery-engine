using System.Activities;
using System.Collections.Generic;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class DeliverInstantEmail : CodeActivity
    {
        
        public InOutArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<DeliveryLeadData> LeadData { get; set; }
        public InArgument<string> EmailBody { get; set; }
        public OutArgument<bool> Success { get; set; }
        
        protected override void Execute(CodeActivityContext context)
        {
            InstantEmailEndpoint endpoint = Endpoint.Get(context) as InstantEmailEndpoint;
            DeliveryLeadData leadData = LeadData.Get(context);
            string emailBody = EmailBody.Get(context);

            //return bool variable
            bool success = false;

           
                Email email = new Email();

                //set type of email body
                email.IsBodyHTML = (endpoint.BodyType == EmailBodyType.HTML);

                //(use configuration manager?)
                //Set SMTP Server from Config 
                email.SMTPHost = System.Configuration.ConfigurationManager.AppSettings["DeliverySMTPServer"];

                //Set FROM parameter from Config
            email.From = leadData.ProductId == 17 || leadData.ProductId == 16
                             ? System.Configuration.ConfigurationManager.AppSettings["DeliveryEmailFromGS"]
                             : System.Configuration.ConfigurationManager.AppSettings["DeliveryEmailFrom"];

            //Set Body, Subject, Addresses
                email.Body = emailBody;
                email.Subject = endpoint.EmailSubject;
                endpoint.IsTest = endpoint.IsTestModeOverRide(leadData);//Added By Bala 11/30/2011

                if (endpoint.IsTest)
                {
                    if (endpoint.TestEmailTo.Length > 0)
                    {
                        email.TO=endpoint.TestEmailTo;
                    }
                    if (endpoint.TestEmailCC.Length > 0)
                    {
                        email.CC=endpoint.TestEmailCC;
                    }
                    if (endpoint.TestEmailBCC.Length > 0)
                    {
                        email.BCC=endpoint.TestEmailBCC;
                    }
                }
                else
                {
                    if (endpoint.LiveEmailTo.Length > 0)
                    {
                        email.TO=endpoint.LiveEmailTo;
                    }
                    if (endpoint.LiveEmailCC.Length > 0)
                    {
                        email.CC=endpoint.LiveEmailCC;
                    }
                    if (endpoint.LiveEmailBCC.Length > 0)
                    {
                        email.BCC = (endpoint.LiveEmailBCC);
                    }
                }

                //Send Email returning success var
                success = email.Send();

          

            // Store the request in the OutArgument
            Success.Set(context, success);
            Endpoint.Set(context, endpoint);//Added By Bala 11/30/2011
        }


    }
}
