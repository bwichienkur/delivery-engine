using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public class CheckActiveEndPoint
    {
        [DataMember]
        public int Active { get; set; }
        [DataMember]
        public string IsActiveEndPointExists { get; set; }
    }
}
