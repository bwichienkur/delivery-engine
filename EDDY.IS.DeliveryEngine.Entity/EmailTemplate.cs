using System.Data;
using System.Runtime.Serialization;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.Nexus.DeliveryEngine.Entity
{
    public class EmailTemplate : CommonEntity
    {
        #region Properties
        [DataMember]
        public int EmailTemplateID { get; set; }

        [DataMember]
        public string EmailTemplateName { get; set; }
        #endregion
        #region Constructor
        public EmailTemplate()
        {

        }

        public EmailTemplate(IDataReader dataReader)
        {

            if (dataReader != null)
            {
                EmailTemplateID = DatabaseUtilities.SetIntValue(dataReader["EmailTemplateID"]);
                EmailTemplateName = DatabaseUtilities.SetStringValue(dataReader["EmailTemplateName"]);

            }
        }
        #endregion 
    }
}
