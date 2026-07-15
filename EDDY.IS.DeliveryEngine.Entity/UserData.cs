using System;
using System.Runtime.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
    [DataContract]
    [Serializable]
    public class UserData
    {

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Value);
        }
    }
}
