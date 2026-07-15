using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.ConditionValidator
{
    public class FtcParameter
    {
        #region properties

        /// <summary>
        /// To get and set DataType
        /// </summary>
        [XmlAttribute("dataType")]
        public string DataType { get; set; }

        /// <summary>
        /// To get and set Id
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// To get and set Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// To get and set Value
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        #endregion

    }
}
