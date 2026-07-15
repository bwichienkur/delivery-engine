using System;
using System.Activities;
using System.Data.SqlClient;
using EDDY.Nexus.DataAccess.DeliveryEngine;
using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;

namespace EDDY.Nexus.BusinessComponent.DeliveryEngine.Activities
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
            IDeliveryEngineDAO deliveryEngineDao = new DeliveryEngineDAO();

            try
            {
                newBatchId = deliveryEngineDao.CreateNewBatchDelivery(deliveryEndpointId, 1, productId, machineName);
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