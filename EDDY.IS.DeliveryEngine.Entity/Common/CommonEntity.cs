using System;
using System.Runtime.Serialization;
using EDDY.IS.DeliveryEngine.Entity.Interface.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Interface.Common;
//using EDDY.Nexus.Entity.Interface.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Interface.Common;

namespace EDDY.IS.DeliveryEngine.Entity.Common
{
    [DataContract]
    [Serializable]
    public abstract class CommonEntity : ICommonEntity
    {
        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
        
        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }
        
        [DataMember]
        public int UpdatedBy { get; set; }
        
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        
        [DataMember]
        public Guid RowGuid { get; set; }

        [DataMember]
        public string CurrentUser { get; set; }

        [DataMember]
        public int CurrentCSR { get; set; }

    }
}
