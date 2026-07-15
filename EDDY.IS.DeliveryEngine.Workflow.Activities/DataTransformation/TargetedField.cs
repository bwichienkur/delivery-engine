using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class TargetedField
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public TargetedField()
        {
            // Default constructor for serialization
        }

        /// <summary>
        /// Constructor to initilise properties based on input
        /// </summary>
        /// <param name="name">Name of the field to target</param>
        /// <param name="dataType">Datatype of the field</param>
        /// <param name="value">Value to be set for the field</param>
        //public TargetedField(string name, string dataType, string value)
        //{
        //    Name = name;
        //    DataType = dataType;
        //    Value = value;
        //}
        # endregion


        # region Properties

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
        /// To get and set DataType
        /// </summary>
        [XmlAttribute("dataType")]
        public string DataType { get; set; }
                
        /// <summary>
        /// To get and set Value
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// To get and set Value
        /// </summary>
        [XmlAttribute("overwritename")]
        public string OverwriteName { get; set; }

        /// <summary>
        /// To get and set Value
        /// </summary>
        [XmlAttribute("maxlength")]
        public string MaxLength { get; set; }
        
        # endregion        
    }
}
