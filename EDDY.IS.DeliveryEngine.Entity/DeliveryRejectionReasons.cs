using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
//using EDDY.Nexus.Entity.Common;
//using EDDY.Nexus.Entity.CoreBusiness;
using System.Xml;


namespace EDDY.IS.DeliveryEngine.Entity
{
    public class DeliveryRejectionReasons
    {
        public int ReasonID { get; set; }
        public string RejectionResponse { get; set; }
        public int AutoReviewID { get; set; }       
                        
        public DeliveryRejectionReasons()
        { 
        
        }

        public DeliveryRejectionReasons(XmlNode aNode)
        {            
            int x = 0;
            string key;
            string value;
            foreach (XmlNode child in aNode.ChildNodes)
            {                
                key = aNode.ChildNodes[x].Name;                
                value = aNode.ChildNodes[x].InnerText;
                                
                if (key == "ReasonID"){
                ReasonID = Convert.ToInt16(value);
                }
                if (key == "RejectionResponse") {
                    RejectionResponse = value;
                }
                if (key == "AutoReviewID") {
                    AutoReviewID = Convert.ToInt16(value);
                }
                               
                x = x + 1;
            }

        }     
                

    }
}
