using System.Xml.Serialization;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class Tasks
    {
        # region Constructor
        /// <summary>
        /// Empty Constructor for serialization
        /// </summary>
        public Tasks()
        {

        }
        #endregion

        # region Properties

        /// <summary>
        /// To get and set AppendNameValuePair
        /// </summary>
        [XmlElement("AppendNameValuePair")]
        public AppendNameValuePair AppendNameValuePair { get; set; }

        /// <summary>
        /// To get and set ChangeFieldNames
        /// </summary>
        [XmlElement("ChangeFieldNames")]
        public ChangeFieldNames ChangeFieldNames { get; set; }

        /// <summary>
        /// To get and set FieldValueMapping
        /// </summary>
        [XmlElement("FieldValueMapping")]
        public FieldValueMapping FieldValueMapping { get; set; }
        
        /// <summary>
        /// To get and set RemoveFields
        /// </summary>
        [XmlElement("RemoveFields")]
        public RemoveFields RemoveFields { get; set; }

        /// <summary>
        /// To get and set FormatTelephoneNumber
        /// </summary>
        [XmlElement("FormatTelephoneNumber")]
        public FormatTelephoneNumber FormatTelephoneNumber { get; set; }

        /// <summary>
        /// To get and set FormatDateTime
        /// </summary>
        [XmlElement("FormatDateTime")]
        public FormatDateTime FormatDateTime { get; set; }

        /// <summary>
        /// To get and set ApplyXsltForXml
        /// </summary>
        [XmlElement("ApplyXsltForXml")]
        public ApplyXsltForXml ApplyXsltForXml { get; set; }

        /// <summary>
        /// To get and set EmailTo
        /// </summary>
        [XmlElement("SetEmailTo")]
        public SetEmailTo SetEmailTo { get; set; }

        /// <summary>
        /// To get and set EmailSubject
        /// </summary>
        [XmlElement("SetEmailSubject")]
        public SetEmailSubject SetEmailSubject { get; set; }

        #endregion
    }
}
