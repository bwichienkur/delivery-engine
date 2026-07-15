using EDDY.IS.DeliveryEngine.DataAccess.DataService;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.UnitTest.MockRepositories;
using EDDY.IS.DeliveryEngine.Workflow.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDY.IS.DeliveryEngine.UnitTest.Activities
{
    [TestClass]
    public class DeliverInstantEmailFixture
    {
        public DeliverInstantEmail deliverInstantEmailActivity;

        [TestInitialize]
        public void DeliverBatchEmail_TestSetup()
        {
            deliverInstantEmailActivity = new DeliverInstantEmail();
            DeliveryEngineDataService.DeliveryEngineDAO = new DeliveryEngineDAOMock();
        }


        [TestMethod]
        public void DeliverInstantEmail_ShouldLogEmailDeliveryAttemptWithFailureReasonIfUnsuccessful()
        {
            //Arrange
            string emailBody = "Test Email Body";
            InstantEmailEndpoint deliveryEndpoint = new InstantEmailEndpoint();
            deliveryEndpoint.EmailSubject = "Test Email Subject";
            deliveryEndpoint.TestEmailTo = "test@blahblah.com";
            deliveryEndpoint.TestEmailCC = string.Empty;
            deliveryEndpoint.TestEmailBCC = string.Empty;

            DeliveryLeadData leadData = new DeliveryLeadData();
            leadData.FirstName = "test";
            leadData.LeadId = 1234567;

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "Endpoint", deliveryEndpoint },
                { "LeadData", leadData },
                { "EmailBody", emailBody }
            };

            //Act
            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(deliverInstantEmailActivity, contextInput);

            //Assert              
            Assert.IsTrue(((DeliveryEngineDAOMock)DeliveryEngineDataService.DeliveryEngineDAO).TempLogMessages.Contains("Failed Email For Lead: 1234567 Attempt: 1 Reason: Failure sending mail."));
            Assert.IsTrue(((DeliveryEngineDAOMock)DeliveryEngineDataService.DeliveryEngineDAO).TempLogMessages.Contains("Failed Email For Lead: 1234567 Attempt: 2 Reason: Failure sending mail."));
            Assert.IsTrue(((DeliveryEngineDAOMock)DeliveryEngineDataService.DeliveryEngineDAO).TempLogMessages.Contains("Failed Email For Lead: 1234567 Attempt: 3 Reason: Failure sending mail."));
        }

        [TestMethod]
        public void DeliverInstantEmail_ShouldLogEmailDeliveryAttemptCountIfCompletelyUnsuccessfulAfterFailure()
        {
            //Arrange
            string emailBody = "Test Email Body";
            InstantEmailEndpoint deliveryEndpoint = new InstantEmailEndpoint();
            deliveryEndpoint.EmailSubject = "Test Email Subject";
            deliveryEndpoint.TestEmailTo = "test@blahblah.com";
            deliveryEndpoint.TestEmailCC = string.Empty;
            deliveryEndpoint.TestEmailBCC = string.Empty;

            DeliveryLeadData leadData = new DeliveryLeadData();
            leadData.FirstName = "test";
            leadData.LeadId = 1234567;

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "Endpoint", deliveryEndpoint },
                { "LeadData", leadData },
                { "EmailBody", emailBody }
            };

            //Act
            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(deliverInstantEmailActivity, contextInput);

            //Assert   
            Assert.IsTrue(((DeliveryEngineDAOMock)DeliveryEngineDataService.DeliveryEngineDAO).TempLogMessages.Contains("Failed Email For Lead: 1234567 after 3 attempts."));
        }
    }
}

