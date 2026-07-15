using System.Activities;
using System.IO;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.Workflow.Activities.Helper;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
using System;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class DeliverBatchEmail : CodeActivity
    {
        public InOutArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<string> BatchFilePath { get; set; }
        public OutArgument<bool> Success { get; set; }
        public InArgument<int> ProductId { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            BatchEmailEndpoint endpoint = Endpoint.Get(context) as BatchEmailEndpoint;
            string batchFilePath = BatchFilePath.Get(context);
            int productId = ProductId.Get(context);

            //return bool variable
            bool success = false;

            Email email = new Email();

            //set type of email body
            email.IsBodyHTML = (endpoint.BodyType == EmailBodyType.HTML);

            //(use configuration manager?)
            //Set SMTP Server from Config 
            email.SMTPHost = System.Configuration.ConfigurationManager.AppSettings["DeliverySMTPServer"];

            //Set FROM parameter from Config
            email.From = productId == 17 || productId == 16
                           ? System.Configuration.ConfigurationManager.AppSettings["DeliveryEmailFromGS"]
                           : System.Configuration.ConfigurationManager.AppSettings["DeliveryEmailFrom"];

            //Set Body, Subject, Addresses
            email.Body = endpoint.EmailBody;
            email.Subject = endpoint.EmailSubject;
            endpoint.IsTest = endpoint.IsTestModeOverRide();
            if (endpoint.IsTest)
            {
                if (endpoint.TestEmailTo.Length > 0)
                {
                    email.TO = endpoint.TestEmailTo;
                }
                if (endpoint.TestEmailCC.Length > 0)
                {
                    email.CC = endpoint.TestEmailCC;
                }
                if (endpoint.TestEmailBCC.Length > 0)
                {
                    email.BCC = endpoint.TestEmailBCC;
                }
            }
            else
            {
                if (endpoint.LiveEmailTo.Length > 0)
                {
                    email.TO = endpoint.LiveEmailTo;
                }
                if (endpoint.LiveEmailCC.Length > 0)
                {
                    email.CC = endpoint.LiveEmailCC;
                }
                if (endpoint.LiveEmailBCC.Length > 0)
                {
                    email.BCC = endpoint.LiveEmailBCC;
                }
            }
            FileInfo f = new FileInfo(batchFilePath);

            if (f.Length==0) email.Subject = "No Leads for this Batch";
            //attach batch file
            email.Attachments.Add(batchFilePath);


            //Send Email returning success var
            if (f.Length == 0 && (productId == 17 || productId == 16))
                success = true;
            else
            {
                int currentRetryCount = 1;
                int emailDeliveryRetryMax = 1;
                Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["EmailRetryMax"], out emailDeliveryRetryMax);
                
                do
                {
                    try
                    {
                        //Send Email returning success var
                        success = email.Send();

                        if (success)
                            break;
                        else
                            currentRetryCount++;
                    }
                    catch (Exception ex)
                    {
                        DeliveryEngineDataService.DeliveryEngineDAO.TempLog(0, 10, 0,
                            "Failed Batch Email Delivery: " + batchFilePath + " Attempt: " + currentRetryCount + " Reason: " + ex.Message, 1, false);

                        currentRetryCount++;
                    }
                } while (currentRetryCount < emailDeliveryRetryMax);

                if (success && currentRetryCount > 1)
                    DeliveryEngineDataService.DeliveryEngineDAO.TempLog(0, 10, 0,
                        "Sent Batch Email Delivery: " + batchFilePath + "after " + currentRetryCount + "attempts.", 1, false);
                else if(!success)
                    DeliveryEngineDataService.DeliveryEngineDAO.TempLog(0, 10, 0,
                        "Failed Batch Email Delivery: " + batchFilePath + "after " + currentRetryCount + "attempts.", 1, false);
            }

            // Store the request in the OutArgument
            Success.Set(context, success);
            Endpoint.Set(context, endpoint); //Added By Bala 11/30/2011
        }

    }
}
