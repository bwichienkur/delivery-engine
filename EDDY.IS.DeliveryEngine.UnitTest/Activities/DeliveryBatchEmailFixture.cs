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
    public class DeliveryBatchEmailFixture
    {
        public DeliverBatchEmail deliverBatchEmailActivity;

        [TestInitialize]
        public void DeliverBatchEmail_TestSetup()
        {
            deliverBatchEmailActivity = new DeliverBatchEmail();
            DeliveryEngineDataService.DeliveryEngineDAO = new DeliveryEngineDAOMock();
        }


        [TestMethod]
        public void DeliverInstantPost_ShouldReplaceVariableURLSegmentIfAllThreeRequiredValuesPresentForURL()
        {
            //Arrange
            string postData = string.Empty;

            BatchEmailEndpoint deliveryEndpoint = new BatchEmailEndpoint();
            deliveryEndpoint.DeliveryEndpointId = 37791;
            deliveryEndpoint.DeliveryDefinitionId = 35684;
            deliveryEndpoint.CSRId = 8625;
            deliveryEndpoint.EndpointName = "Kansas State Polytechnic EndPoint";
            deliveryEndpoint.EndpointDetailXML = "<DeliveryEndpointDetail DeliveryTypeId=\"1\"><Fields><Field FieldName=\"VariableURLSegmentLeadDataValue\" Value=\"emailaddress\" /><Field FieldName=\"VariableURLSegmentPlaceHolder\" Value=\"replacethis @withcontactemail.com\" /><Field FieldName=\"VariableURLSegment\" Value=\"true\" /><Field FieldName=\"LivePostUrl\" Value=\"https://api.hubapi.com/contacts/v1/contact/createOrUpdate/email/replacethis@withcontactemail.com/?hapikey=25fe0d45-80eb-40d3-bdde-b69f27ae3bf3\" /><Field FieldName=\"TestPostUrl\" Value=\"http://nexprod/eddy.admin.webapplication/post_test.aspx\" /><Field FieldName=\"PostMethod\" Value=\"POST\" /><Field FieldName=\"PostContentType\" Value=\"JSON\" /><Field FieldName=\"SuccessResponseString\" Value=\"true\" /><Field FieldName=\"FailureResponseString\" Value=\"false\" /><Field FieldName=\"ForceRespostString\" Value=\"\" /><Field FieldName=\"SoapAction\" Value=\"\" /><Field FieldName=\"MaxRetryAttempts\" Value=\"4\" /><Field FieldName=\"RetryDelayInHours\" Value=\"4\" /><Field FieldName=\"IsTestURL\" Value=\"false\" /><Field FieldName=\"IsInternalDelivery\" Value=\"false\" /><Field FieldName=\"IsPrimary\" Value=\"true\" /></Fields></DeliveryEndpointDetail>";
            deliveryEndpoint.DeliveryTypeId = 1;
            deliveryEndpoint.IsTest = true;
            deliveryEndpoint.DeliveryType = LeadDeliveryType.InstantPost;
            //deliveryEndpoint.TestPostUrl = "http://nexprod/eddy.admin.webapplication/email/replacethis@withcontactemail.com/post_test.aspx";
            //deliveryEndpoint.LivePostUrl = "http://nexprod/eddy.admin.webapplication/email/replacethis@withcontactemail.com/post_live.aspx";
            deliveryEndpoint.EndPointDetailDictionary = new Dictionary<string, string>();
            deliveryEndpoint.EndPointDetailDictionary.Add("VariableURLSegmentLeadDataValue", "EmailAddress");
            deliveryEndpoint.EndPointDetailDictionary.Add("VariableURLSegmentPlaceHolder", "replacethis@withcontactemail.com");
            deliveryEndpoint.EndPointDetailDictionary.Add("VariableURLSegment", "true");
            deliveryEndpoint.EndPointDetailDictionary.Add("LivePostUrl", "http://nexprod/eddy.admin.webapplication/email/replacethis@withcontactemail.com/post_live.aspx");
            deliveryEndpoint.EndPointDetailDictionary.Add("TestPostUrl", "http://nexprod/eddy.admin.webapplication/email/replacethis@withcontactemail.com/post_test.aspx");
            deliveryEndpoint.EndPointDetailDictionary.Add("PostMethod", "POST");
            deliveryEndpoint.EndPointDetailDictionary.Add("PostContentType", "JSON");
            deliveryEndpoint.EndPointDetailDictionary.Add("SuccessResponseString", "true");
            deliveryEndpoint.EndPointDetailDictionary.Add("FailureResponseString", "false");
            deliveryEndpoint.EndPointDetailDictionary.Add("ForceRespostString", "");
            deliveryEndpoint.EndPointDetailDictionary.Add("SoapAction", "");
            deliveryEndpoint.EndPointDetailDictionary.Add("MaxRetryAttempts", "4");
            deliveryEndpoint.EndPointDetailDictionary.Add("RetryDelayInHours", "4");
            deliveryEndpoint.EndPointDetailDictionary.Add("IsTestURL", "false");
            deliveryEndpoint.EndPointDetailDictionary.Add("IsInternalDelivery", "false");
            deliveryEndpoint.EndPointDetailDictionary.Add("IsPrimary", "true");

            DeliveryLeadData leadData = new DeliveryLeadData();
            leadData.FirstName = "Test";
            leadData.LastName = "User";
            leadData.EmailAddress = "Test.User@test.com";
            leadData.Phone1 = "5554234633";

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "Endpoint", deliveryEndpoint },
                { "PostData", postData },
                { "LeadData", leadData }
            };

            //Act
            //IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(deliverInstantPostActivity, contextInput);

            //Assert
            //deliveryEndpoint = (InstantPostEndpoint)contextOutput["Endpoint"];
            //Assert.AreEqual("http://nexprod/eddy.admin.webapplication/email/Test.User@test.com/post_test.aspx", deliveryEndpoint.TestPostUrl);
        }

    }
}
