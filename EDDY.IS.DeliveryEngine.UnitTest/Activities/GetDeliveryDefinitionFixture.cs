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
    public class GetDeliveryDefinitionFixture
    {
        public GetDeliveryDefinition getDeliveryDefinitionActivity;

        [TestInitialize]
        public void GetDeliveryDefinition_TestSetup()
        {
            getDeliveryDefinitionActivity = new GetDeliveryDefinition();
        }

        [TestMethod]
        public void GetDeliveryDefinition_ShouldAssignCorrectDefinitionToLeadIfLeadMeetsDeliveryCriteria()
        {
            //Arrange
            DeliveryDefinition deliveryDefinition = new DeliveryDefinition();
            DeliveryLeadData leadData = LeadDataService.LeadDAO.GetDeliveryLeadDataByLeadId(15278052);
            
            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "LeadData", leadData }                
            };

            //Act
            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(getDeliveryDefinitionActivity, contextInput);
            
            //Assert
            Assert.IsTrue(contextOutput.Where(x => x.Key.Equals("DeliveryDefinition")).Count() > 0);
            Assert.AreEqual(37013, ((DeliveryDefinition)contextOutput["DeliveryDefinition"]).DeliveryDefinitionId);
        }

        [TestMethod]
        public void GetDeliveryDefinition_ShouldAssignNoDefinitionToLeadIfDoesNotLeadMeetsDeliveryCriteria()
        {
            DeliveryDefinition deliveryDefinition = new DeliveryDefinition();
            DeliveryLeadData leadData = LeadDataService.LeadDAO.GetDeliveryLeadDataByLeadId(15267993);
            leadData.LeadCreationTypeId = 12;

            IDictionary<string, object> contextInput = new Dictionary<string, object>
            {
                { "LeadData", leadData }
            };

            IDictionary<string, object> contextOutput = WorkflowInvoker.Invoke(getDeliveryDefinitionActivity, contextInput);
            Assert.IsTrue(contextOutput["DeliveryDefinition"] == null);            
        }
    }
}
