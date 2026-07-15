using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryDefinitionItem:CommonEntity
    {
        [DataMember]
        public int CrID { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public string DeliveryName { get; set; }
        [DataMember]
        public string ConditionXML { get; set; }
    }
}
