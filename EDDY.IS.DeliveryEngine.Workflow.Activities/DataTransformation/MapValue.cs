using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class MapValue
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public MapValue()
        {
            // Default constructor for serialization
        }

        /// <summary>
        /// Constructor to initilise properties based on input
        /// </summary>
        /// <param name="oldValue">Old value to check in target string</param>
        /// <param name="newValue">New value to update</param>
        //public MapValue(string oldValue, string newValue)
        //{
        //    OldValue = oldValue;
        //    NewValue = newValue;
        //}
        #endregion


        # region Properties

        /// <summary>
        /// To get and set Id
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// To get and set OldValue
        /// </summary>
        [XmlAttribute("oldValue")]
        public string OldValue { get; set; }

        /// <summary>
        /// To get and set NewValue
        /// </summary>
        [XmlAttribute("newValue")]
        public string NewValue { get; set; }

        #endregion

    }
}
