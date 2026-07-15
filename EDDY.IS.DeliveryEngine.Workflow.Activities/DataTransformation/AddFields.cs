using System;
using System.Collections.Generic;
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
    public class AddFields : IDataTransformationTask
    {
        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public AddFields()
        {
            //<AddFields>
            //  <TargetedFields>
            //    <TargetedField name='AddressLine2' value='Address2' dataType='string' overwritename='HomeAddress' maxlength='50'/>
            //  </TargetedFields>
            //</AddFields>
        }


        #endregion

        # region Properties
        /// <summary>
        /// To get and set TargetedFields
        /// </summary>
        [XmlArray("TargetedFields")]
        [XmlArrayItem("TargetedField")]
        public List<TargetedField> TargetedFields { get; set; }
        #endregion

        # region IDataTransformationTask Methods
        /// <summary>
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            var result = new Dictionary<string, object>();
            try
            {
                log.Append("\r\n - Add Fields transformation started \r\n");

               
                foreach (var field in TargetedFields)
                {

                    var originalLabel = field.Name;
                    var overwriteLabel = string.IsNullOrWhiteSpace(field.OverwriteName)
                                             ? originalLabel
                                             : field.OverwriteName;
                    var value = field.Value;
                    var fieldType = field.DataType;

                    //log.Append(string.Format("|{0}=>{1}={2} \r\n", originalLabel,
                    //                                     overwriteLabel, value));

                    switch (fieldType.ToLower())
                    {
                            //Add to result with existing value
                        case "field":
                            if (rawData.ContainsKey(originalLabel))
                            {
                                value = string.IsNullOrWhiteSpace(rawData[originalLabel].ToString())
                                            ? value
                                            : rawData[originalLabel].ToString();

                                log.Append(string.Format("Added value: {0}=>{1}={2}|", originalLabel,
                                                         overwriteLabel, value));
                                if (result.ContainsKey(overwriteLabel)) result.Remove(overwriteLabel);
                                result.Add(overwriteLabel, value);
                            }
                            break;

                            //add a constant to result
                        case "local":
                            log.Append(string.Format("Added constant: {0}=>{1}={2}|", originalLabel,
                                                     overwriteLabel, value));
                            if (result.ContainsKey(overwriteLabel)) result.Remove(overwriteLabel);
                            result.Add(overwriteLabel, value);
                            break;

                            //stored procedure result
                        case "storedprocedure":
                            log.Append(string.Format("Added field sp: {0}=>{1}={2}|", originalLabel,
                                                     overwriteLabel, value));
                            if (result.ContainsKey(overwriteLabel)) result.Remove(overwriteLabel);

                            //validate
                            Int64 leadId;
                            Int64.TryParse(rawData["LeadId"].ToString(), out leadId);
                            if (leadId == 0)
                                throw new ArgumentException(
                                    "AddFields for sp transform error. LeadId is not found in the raw lead data");
                            if (String.IsNullOrEmpty(value))
                                throw new ArgumentException("AddFields for sp transform error. SP name is not defined.");

                            //Get value
                            string returnedValue = DataTransformationDataService.DataTransformationDAO.GetStoredProcResult(value, leadId);
                            result.Add(overwriteLabel, returnedValue);
                            break;
                    }

                }

                log.Append(" - AddFields transformation end \r\n");
            }
            catch (Exception ex)
            {
                log.Append(" - AddFields  transformation error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }

            if (rawData.ContainsKey("LiveEmailTo"))
                result.Add("LiveEmailTo", rawData["LiveEmailTo"].ToString());

            return result;
        }
        # endregion

    }
}
