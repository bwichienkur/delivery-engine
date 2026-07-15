
using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity.ConditionValidator
{

    public class Condition
    {
        # region Properties
        /// <summary>
        /// To get and set FieldsToCompare
        /// </summary>
        [XmlArray("FieldsToCompare")]
        [XmlArrayItem("FieldToCompare")]
        public FieldToCompare[] FieldsToCompare { get; set; }
        #endregion

    }
}
