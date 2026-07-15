using System.Runtime.Serialization;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Entity.ConditionValidator
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public bool IsConditionMet { get; set; }

        [DataMember]
        public StringBuilder Log { get; set; }
    }
}
