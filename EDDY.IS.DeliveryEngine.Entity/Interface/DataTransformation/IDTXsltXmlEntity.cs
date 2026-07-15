using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface IDTXsltXmlEntity
    {
        int TaskID { get; set; }
        int TaskXMLID { get; set; }
        string TaskXML { get; set; }
        string TaskDesc { get; set; }
        string UpdatedByName { get; set; }
        string XSLT { get; set; }
    }
}
