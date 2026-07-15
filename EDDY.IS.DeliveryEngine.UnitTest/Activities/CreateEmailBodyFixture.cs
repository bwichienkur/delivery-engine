using EDDY.IS.DeliveryEngine.DataAccess.DataService;
using EDDY.IS.DeliveryEngine.Entity;
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
    public class CreateEmailBodyFixture
    {
        public CreateEmailBody createEmailBody;

        [TestInitialize]
        public void DeliverBatchEmail_TestSetup()
        {
            createEmailBody = new CreateEmailBody();
        }

        [TestMethod]
        public void CreateEmailBody_ShouldCreateEmailBody()
        {
            //Arrange
            string postData = string.Empty;

            DeliveryEndpoint endpoint = null;
            endpoint = DeliveryEngineDataService.DeliveryEngineDAO.GetEndpointById(37925);

            DeliveryLeadData deliveryLeadData = new DeliveryLeadData();
            deliveryLeadData.TransformedNameValuePairs = new Dictionary<string, object>();
            deliveryLeadData.TransformedNameValuePairs.Add("Lead Date", "2019-06-06T19:33:35.230");
            deliveryLeadData.TransformedNameValuePairs.Add("First Name", "Latesha");
            deliveryLeadData.TransformedNameValuePairs.Add("Last Name", "Anderson");
            deliveryLeadData.TransformedNameValuePairs.Add("Email", "reillydevine0615@gmail.com");
            deliveryLeadData.TransformedNameValuePairs.Add("Street Address", "18425 kitt ridge st apt 414");
            deliveryLeadData.TransformedNameValuePairs.Add("City", "Reseda");
            deliveryLeadData.TransformedNameValuePairs.Add("State", "CA");
            deliveryLeadData.TransformedNameValuePairs.Add("Zip/Postal Code", "91335");
            deliveryLeadData.TransformedNameValuePairs.Add("Primary Phone", "8186267020");
            deliveryLeadData.TransformedNameValuePairs.Add("High School Grad Year", "2004");
            deliveryLeadData.TransformedNameValuePairs.Add("Age", "33");
            deliveryLeadData.TransformedNameValuePairs.Add("Start Date", "1-3 Months");
            deliveryLeadData.TransformedNameValuePairs.Add("Program", "Business Management and Accounting");
            deliveryLeadData.TransformedNameValuePairs.Add("Education Level", "Some College, 1-29 Credits");
            deliveryLeadData.TransformedNameValuePairs.Add("Campus", "Panorama City, CA");

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "Endpoint", endpoint },
                { "LeadData", deliveryLeadData }
            };

            //Act
            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(createEmailBody, contextInput);

            //Assert
            //deliveryEndpoint = (InstantPostEndpoint)contextOutput["Endpoint"];
            //Assert.AreEqual("http://nexprod/eddy.admin.webapplication/email/Test.User@test.com/post_test.aspx", deliveryEndpoint.TestPostUrl);
        }

        private DeliveryLeadData[] GetArrayFromList(List<DeliveryLeadData> list)
        {
            DeliveryLeadData[] returnArray = new DeliveryLeadData[list.Count];
            int index = 0;

            foreach (DeliveryLeadData item in list)
            {
                returnArray[index] = item;
                index++;
            }

            return returnArray;
        }
    }
}

