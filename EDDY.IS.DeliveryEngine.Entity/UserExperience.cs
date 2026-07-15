using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
    [DataContract]
    [Serializable]
    public class UserExperience
    {

        [DataMember]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute("value")]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Value);
        }
    }

    [DataContract]
    [Serializable]
    [XmlRoot("metadata")]
    public class UserExperienceList
    {
        public UserExperienceList() { Items = new List<UserExperience>(); }
        [XmlElement("ue")]
        public List<UserExperience> Items { get; set; }
    }

}
