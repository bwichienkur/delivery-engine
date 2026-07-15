using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDY.IS.DeliveryEngine.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EDDY.IS.DeliveryEngine.DataAccess.Common;

namespace EDDY.IS.DeliveryEngine.UnitTest.MockRepositories
{
    public class LeadDAOMock : BaseDataSource, ILeadDAO
    {
        public Dictionary<string, object> GetAdditionalFieldsDictionaryFromXMLResults;
        public DeliveryLeadData GetDeliveryLeadDataByLeadIdResults;
        public DeliveryLeadData GetRealtimeDeliveryLeadDataByLeadIdResults;

        public Dictionary<string, object> GetAdditionalFieldsDictionaryFromXML(string additionalFieldsXML)
        {
            return GetAdditionalFieldsDictionaryFromXMLResults;
        }

        public DeliveryLeadData GetDeliveryLeadDataByLeadId(int leadId)
        {
            return GetDeliveryLeadDataByLeadIdResults;
        }

        public DeliveryLeadData GetRealtimeDeliveryLeadDataByLeadId(int leadId)
        {
            return GetRealtimeDeliveryLeadDataByLeadIdResults;
        }
    }
}
