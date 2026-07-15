using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Workflow.Activities.Cap.Interface;
using EDDY.IS.DeliveryEngine.Entity.Cap;
using EDDY.IS.DeliveryEngine.Entity;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.BusinessComponent.Interface.CoreBusiness;
//using EDDY.Nexus.DataAccess.CoreBusiness;
//using EDDY.Nexus.DataAccess.Interface.CoreBusiness;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Cap
{
    public class CapDistributionComponent : ICapDistributionComponent
    {

        public void CreateCap(CapDistributionEntity capEntity)
        {
            CapDistributionDataService.CapDistributionDAO.CreateCAP(capEntity);
        }


        public void CreateCapLevel(CapLevelEntity capLevelEntity)
        {
            CapDistributionDataService.CapDistributionDAO.CreateCapLevel(capLevelEntity);
        }

        public List<GetCapEntity> GetCap()
        {
            List<GetCapEntity> listGetCap = new List<GetCapEntity>();
            CapDistributionDataService.CapDistributionDAO.GetCap();
            return listGetCap;
        }

        public void EditCap(CapDistributionEntity capEntity)
        {
            CapDistributionDataService.CapDistributionDAO.EditCap(capEntity);
        }

        public bool UpdateCapValueByCSRId(DeliveryLeadData deliveryData)
        {

            bool chkValue = false;
            chkValue = CapDistributionDataService.CapDistributionDAO.UpdateCapValueByCSRId(deliveryData);
            return chkValue;
        }


        public void ProcessCap(DeliveryLeadData deliveryData)
        {
            CapDistributionDataService.CapDistributionDAO.ProcessCap(deliveryData);
        }

       
    }
}
