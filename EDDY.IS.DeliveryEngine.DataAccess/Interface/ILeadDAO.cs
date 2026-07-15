using System.Collections.Generic;
using EDDY.IS.DeliveryEngine.DataAccess.Interface.Common;
using EDDY.IS.DeliveryEngine.Entity;
//using EDDY.Nexus.DataAccess.Interface.Common;
//using EDDY.Nexus.Entity.CoreBusiness;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface ILeadDAO : IBaseDataSource
    {
        DeliveryLeadData GetDeliveryLeadDataByLeadId(int leadId);
        DeliveryLeadData GetRealtimeDeliveryLeadDataByLeadId(int leadId);
        Dictionary<string, object> GetAdditionalFieldsDictionaryFromXML(string additionalFieldsXML);       
    }
}
