using System.Runtime.Serialization;
//using EDDY.Nexus.Entity.Common;
using EDDY.IS.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity.Common
{
    [DataContract]
    public class RequestHeader : CommonEntity
    {
          [DataMember]
          public int ApplicationId { get; set; }
          [DataMember]
          public int SiteId { get; set; }
    }
}



