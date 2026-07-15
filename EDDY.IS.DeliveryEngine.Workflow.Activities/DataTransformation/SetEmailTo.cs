using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DataTransformation;
//using EDDY.Nexus.DataAccess.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class SetEmailTo:IDataTransformationTask
    {
               # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public SetEmailTo()
        { 
            //<FormatDateTime>
            //  <TargetedFields>
            //    <TargetedField name='myDateTime' value='mm-dd-yyyy' />
            //    <TargetedField name='leadDate' value='dd-mm-yy hh:mm:ss' />
            //  </TargetedFields>
            //</FormatDateTime>        
        }
        #endregion

        # region Properties
        /// <summary>
        /// To get and set TargetedFields
        /// </summary>
        [XmlArray("TargetedFields")]
        [XmlArrayItem("TargetedField")]
        public TargetedField[] TargetedFields { get; set; }
        #endregion

        #region IDataTransformationTask Members
        /// <summary>
        /// Transforms the rawData dictionary with SetEmailTo rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            string fname = string.Empty;
            string fvalue = string.Empty;
            try
            {
                log.Append("\r\n - Email Routing transformation started \r\n");
                //This transforms the value of the Email To depends on the condition
                foreach (TargetedField targetedField in TargetedFields)
                {
                    fname = targetedField.Name;
                    if (targetedField.Name.ToLower().Equals("liveemailto"))
                    {
                        fvalue = targetedField.Value;

                        if (fvalue == "SystemEmailLookup")
                        {
                            Int64 leadId;
                            Int64.TryParse(rawData["LeadId"].ToString(), out leadId);
                            if (leadId == 0)
                                throw new ArgumentException(
                                    "AppendNameValuePair for sp transform error. LeadId is not found in the raw lead data");

                            string returnedValue = DataTransformationDataService.DataTransformationDAO.GetStoredProcResult("Eddy_DE_LookUpDeliveryEmail", leadId);
                            fvalue = returnedValue;
                        }

                        if (string.IsNullOrEmpty(fvalue)) 
                            rawData.Remove(targetedField.Name);
                        else
                            rawData[targetedField.Name] = fvalue;
                    }
                }
                log.Append(" - Live Email To transformation end \r\n");
            }
            catch (System.FormatException exc)
            {
                log.Append("LeadFieldName:" + fname + " Leive Email To: " + fvalue + "- SetEmailTo transformation error  \r\n");
                throw new FormatException("LeadFieldName:" + fname + " FieldFormat: " + fvalue +
                                            "- SetEmailTo transformation error  \r\n");
            }
            catch (Exception ex)
            {
                log.Append("LeadFieldName:" + fname + " EmailTo: " + fvalue + "- SetEmailTo transformation error  \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }

        #endregion

    }
}
