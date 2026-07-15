using System;
using System.Activities;
using System.Collections.Generic;
//using EDDY.Nexus.DataAccess.DeliveryEngine;
//using EDDY.Nexus.DataAccess.Interface.DeliveryEngine;
//using EDDY.Nexus.Entity.CoreBusiness;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities
{
    public sealed class GetLeadsForBatch : CodeActivity
    {
        public InArgument<Int32> BatchDeliveryId { get; set; }
        public OutArgument<DeliveryLeadData[]> DeliveryLeadDataArray { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            List<DeliveryLeadData> deliveryLeadData = null;
            DeliveryLeadData[] arrayToReturn = null;

            int batchDeliveryId = BatchDeliveryId.Get(context);
            deliveryLeadData = DeliveryEngineDataService.DeliveryEngineDAO.GetLeadsForBatch(batchDeliveryId);
            arrayToReturn = GetArrayFromList(deliveryLeadData);

            //store 
            DeliveryLeadDataArray.Set(context, arrayToReturn);

        }

        private DeliveryLeadData[] GetArrayFromList(List<DeliveryLeadData> list)
        {
            DeliveryLeadData[] returnArray = new DeliveryLeadData[list.Count];
            int index = 0;

           
               
                foreach(DeliveryLeadData item in list)
                {
                    returnArray[index] = item;
                    index++;
                }
          

            return returnArray;

        }

    }
}