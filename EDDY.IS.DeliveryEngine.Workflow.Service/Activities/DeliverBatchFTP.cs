using System.Activities;
using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.Nexus.Entity.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
{
    public sealed class DeliverBatchFTP : CodeActivity
    {
        public InOutArgument<DeliveryEndpoint> Endpoint { get; set; }
        public InArgument<string> BatchFilePath { get; set; }
        public OutArgument<bool> Success { get; set; }

        
        protected override void Execute(CodeActivityContext context)
        {
            BatchFtpEndpoint endpoint = Endpoint.Get(context) as BatchFtpEndpoint;
            string batchFilePath = BatchFilePath.Get(context);
            //DeliveryLeadData leadData = LeadData.Get(context);
            //return bool variable
            bool success = false;

            endpoint.IsTest = endpoint.IsTestModeOverRide();
            string postUrl;
            if (endpoint.IsTest)
            {
                postUrl = endpoint.TestURL;
            }
            else
            {
                postUrl = endpoint.URI;
            }
           
                //New FTPSender
                FTPSender ftpSender = new FTPSender();

                //Settings
                ftpSender.FTPServerIP = postUrl;
                ftpSender.UserId = endpoint.UserName;
                ftpSender.Password = endpoint.Password;
                ftpSender.FileName = batchFilePath;
                ftpSender.FTPDirectory = endpoint.RemoteFolder;

                //attempt upload and return success var
                success = ftpSender.Upload();

           

            // Store the request in the OutArgument
            Success.Set(context, success);
            Endpoint.Set(context, endpoint);//Added By Bala 11/30/2011
        }

    }
}
