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
    public class CreateBatchLeadFileFixture
    {
        public CreateBatchLeadFile createBatchLeadFile;

        [TestInitialize]
        public void DeliverBatchEmail_TestSetup()
        {
            createBatchLeadFile = new CreateBatchLeadFile();            
        }


        [TestMethod]
        public void DeliverInstantPost_ShouldReplaceVariableURLSegmentIfAllThreeRequiredValuesPresentForURL()
        {
            //Arrange
            string postData = string.Empty;

            DeliveryEndpoint returnEndpoint = null;            
            returnEndpoint = DeliveryEngineDataService.DeliveryEngineDAO.GetEndpointById(37628);

            int batchDeliveryId = 101723;
            DeliveryLeadData[] leadData = null;
            List<DeliveryLeadData> deliveryLeadData = DeliveryEngineDataService.DeliveryEngineDAO.GetLeadsForBatch(batchDeliveryId);
            leadData = GetArrayFromList(deliveryLeadData);          

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "BatchDeliveryId", batchDeliveryId },
                { "DeliveryEndpoint", returnEndpoint },
                { "LeadArray", leadData }
            };

            //Act
            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(createBatchLeadFile, contextInput);

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
