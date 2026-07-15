using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.Entity.Cap;
using EDDY.IS.DeliveryEngine.Entity;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Cap.Interface
{
    public interface ICapDistributionComponent
    {
        void CreateCap(CapDistributionEntity capEntity);

        List<GetCapEntity> GetCap();

        void EditCap(CapDistributionEntity capEntity);

        bool UpdateCapValueByCSRId(DeliveryLeadData deliveryData);

        void CreateCapLevel(CapLevelEntity capLevelEntity);
        void ProcessCap(DeliveryLeadData deliveryData);

    }
}
