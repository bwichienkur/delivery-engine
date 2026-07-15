using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class FormatDateTime : IDataTransformationTask
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public FormatDateTime()
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
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            var fname = string.Empty;
            var fvalue = string.Empty;
            try
            {
                log.Append("\r\n - FormatDateTime transformation started \r\n");
                //Formats the date time string to given format
                foreach (var targetedField in TargetedFields)
                {
                    fname = targetedField.Name;
                    if (!rawData.ContainsKey(targetedField.Name)) continue;

                    fvalue = targetedField.Value;
                    var output = fvalue;
                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        output = String.Format("{0:" + fvalue + "}",
                                               DateTime.Parse(rawData[targetedField.Name].ToString()));


                    }
                    log.Append(" - " + rawData[targetedField.Name] + " formatted as " + output + " \r\n");
                    rawData[targetedField.Name] = output;
                }
                log.Append(" - FormatDateTime transformation end \r\n");
            }
            catch (System.FormatException exc)
            {
                log.Append("LeadFieldName:" + fname + " FieldFormat: " + fvalue + "- FormatTelephoneNumber transformation error  \r\n");
                throw new FormatException("LeadFieldName:" + fname + " FieldFormat: " + fvalue +
                                            "- FormatTelephoneNumber transformation error  \r\n");
            }
            catch (Exception ex)
            {
                log.Append("LeadFieldName:" + fname + " FieldFormat: " + fvalue + "- FormatTelephoneNumber transformation error  \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }

        #endregion
    }
}
