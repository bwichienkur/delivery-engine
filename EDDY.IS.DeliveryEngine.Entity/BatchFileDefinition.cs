using System.Xml.Serialization;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public enum BatchFileType
    {
        Undefined = 0,
        Delimited = 1,
        XML = 2
    }

    public class BatchFileDefinition
    {

        [XmlAttribute("BatchFileTypeString")]
        public string BatchFileTypeString { get; set; }

        [XmlAttribute("BatchFileType")]
        public BatchFileType BatchFileType 
        {
            get
            {
                if (BatchFileTypeString == null)
                { 
                    return BatchFileType.Undefined; 
                }

                switch (BatchFileTypeString.ToLower().Trim())
                {
                    case "delimited":
                    case "csv":
                    case "pipe":
                    case "comma":
                        {
                            return BatchFileType.Delimited;
                        }
                    case "xml":
                        {
                            return BatchFileType.XML;
                        }
                    default:
                        {
                            return BatchFileType.Undefined;
                        }
                }
            }
        }

        [XmlAttribute("ShowHeading")]
        public bool ShowHeading { get; set; }

        [XmlAttribute("FileExtension")]
        public string FileExtension { get; set; }

        [XmlAttribute("FileNameRegExp")]
        public string FileNameRegExp { get; set; }

        [XmlAttribute("FieldDelimiter")]
        public string FieldDelimiter { get; set; }

        [XmlAttribute("TextQualifier")]
        public string TextQualifier { get; set; }

        [XmlAttribute("RowDelimiter")]
        public string RowDelimiter { get; set; }

        public string XSL { get; set; }
    }
}
