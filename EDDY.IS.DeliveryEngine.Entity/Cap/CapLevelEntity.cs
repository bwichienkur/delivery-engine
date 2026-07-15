using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity.Cap
{
    public class CapLevelEntity : CommonEntity
    {
        public CapLevelEntity()
        {
        }

        [DataMember]
        public int CapId { get; set; }
        [DataMember]
        public string CapLevel { get; set; }
        [DataMember]
        public int EntityMetaId { get; set; }
        [DataMember]
        public int EntityId { get; set; }
        [DataMember]
        public int Status { get; set; }

    }
}
