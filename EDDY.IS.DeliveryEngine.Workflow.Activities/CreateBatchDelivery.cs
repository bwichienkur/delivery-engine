using System;
using System.Activities;
using System.Data.SqlClient;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class CreateBatchDelivery : CodeActivity
    {
        public InArgument<int> DeliveryEndpointId { get; set; }
        public OutArgument<int> BatchDeliveryId { get; set; }
        public InArgument<int> ProductId { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            var deliveryEndpointId = DeliveryEndpointId.Get(context);
            var newBatchId = 0;
            var productId = ProductId.Get(context);
            var machineName = System.Environment.MachineName;

            try
            {
                newBatchId = DeliveryEngineDataService.DeliveryEngineDAO.CreateNewBatchDelivery(deliveryEndpointId, 1, productId, machineName);
            }
            catch (SqlException ex)
            {
                throw new ArgumentException(".Invalid endpoint id: " + deliveryEndpointId + ". " + ex.Message);
            }

            //output new batch id
            BatchDeliveryId.Set(context, newBatchId);

        }
    }
}