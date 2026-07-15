using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class SetEmailSubject : IDataTransformationTask 
    {
        
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public SetEmailSubject()
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
        /// Transforms the rawData dictionary with SetEmailSubject rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            string fname = string.Empty;
            string fvalue = string.Empty;
            try
            {
                log.Append("Email Subject transformation started.");
                //This transforms the value of the email subject depends on the condition
                foreach (TargetedField targetedField in TargetedFields)
                {
                    fname = targetedField.Name;
                    if (targetedField.Name.ToLower().Equals("emailsubject"))
                    {
                        fvalue = targetedField.Value;
                        rawData[targetedField.Name] = fvalue;
                    }
                }
                log.Append("Email Subject transformation end.");
            }
            catch (System.FormatException exc)
            {
                log.Append("LeadFieldName:" + fname + " EmailSubject: " + fvalue + "- SetEmailSubject transformation error  \r\n");
                throw new FormatException("LeadFieldName:" + fname + " FieldFormat: " + fvalue +
                                            "- SetEmailSubject transformation error  \r\n");
            }
            catch (Exception ex)
            {
                log.Append("LeadFieldName:" + fname + " EmailSubject: " + fvalue + "- SetEmailSubject transformation error  \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }

        #endregion
    }
}
