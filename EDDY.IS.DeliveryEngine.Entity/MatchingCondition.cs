using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.ConditionValidator
{
    public class MatchingCondition
    {
        #region Properties
        [DataMember]
        public int ConditionId { get; set; }

        [DataMember]
        public string ConditionName { get; set; }

        [DataMember]
        public int ConditionEventId { get; set; }

        [DataMember]
        public string ConditionXML { get; set; }

        [DataMember]
        public bool ConditionMet { get; set; }
        #endregion
    }
}
