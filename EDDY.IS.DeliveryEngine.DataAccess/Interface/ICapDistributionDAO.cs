using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Entity.Cap;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.Entity.CoreBusiness;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface ICapDistributionDAO
    {
        void CreateCAP(CapDistributionEntity capEntity);
        List<GetCapEntity> GetCap();
        void EditCap(CapDistributionEntity capEntity);
        bool UpdateCapValueByCSRId(DeliveryLeadData deliveryData);

        void CreateCapLevel(CapLevelEntity capLevelEntity);

        void ProcessCap(DeliveryLeadData deliveryData);
    }
}