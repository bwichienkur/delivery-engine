using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
//using EDDY.Nexus.BusinessComponent.Interface.DataTransformation;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.Interface;
using EDDY.IS.DeliveryEngine.DataAccess;
using EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface;
using EDDY.IS.DeliveryEngine.DataAccess.DataService;
//using EDDY.Nexus.DataAccess.DataTransformation;
//using EDDY.Nexus.DataAccess.Interface.DataTransformation;
//using EDDY.Nexus.Entity.Interface.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation
{
    public class FieldValueMapping : IDataTransformationTask
    {
        # region Private Variables
        private DataSourceType _DataSourceType;
        private string _DataSourceTypeString;
        private DataType _DataType;
        private string _DataTypeString;
        #endregion

        # region Constructor
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public FieldValueMapping()
        {
            //<FieldValueMapping FieldName="ProgramOfInterest" DataType="string"  targetDataType="string">
            //  <MapValues>
            //    <MapValue oldValue="10624" newValue="443"/>
            //    <MapValue oldValue="11037" newValue="750" />
            //    <MapValue oldValue="12155" newValue="752" />
            //  </MapValues>
            //</FieldValueMapping>    
        }

        /// <summary>
        /// Constructor for populating XML as object
        /// </summary>
        /// <param name="ruleNode">XMLDocument will be parsed and values will be set to properties</param>
        //public FieldValueMapping(XmlDocument ruleNode)
        //{
        //    //<FieldValueMapping FieldName="ProgramOfInterest" DataType="string"  targetDataType="string">
        //    //  <MapValues>
        //    //    <MapValue oldValue="10624" newValue="443"/>
        //    //    <MapValue oldValue="11037" newValue="750" />
        //    //    <MapValue oldValue="12155" newValue="752" />
        //    //  </MapValues>
        //    //</FieldValueMapping>

        //    int index = 0;
        //    if (ruleNode != null)
        //    {
        //        XmlNodeList DataTransformationTaskNodes = ruleNode.SelectNodes(@"//FieldValueMapping");

        //        FieldName = DataTransformationTaskNodes[0].Attributes["FieldName"].InnerXml.ToString();
        //        DataTypeString = DataTransformationTaskNodes[0].Attributes["DataType"].InnerXml.ToString();

        //        XmlNodeList MapValueNodes = ruleNode.SelectNodes(@"FieldValueMapping/MapValues/MapValue");

        //        string oldValue = string.Empty;
        //        string newValue = string.Empty;
        //        MapValues = new MapValue[MapValueNodes.Count];
        //        foreach (XmlNode node in MapValueNodes)
        //        {
        //            oldValue = MapValueNodes[index].Attributes["oldValue"].InnerXml.ToString();
        //            newValue = MapValueNodes[index].Attributes["newValue"].InnerXml.ToString();

        //            MapValues[index] = new MapValue(oldValue, newValue);
        //            index++;
        //        }
        //    }
        //}
        #endregion

        # region Properties
        /// <summary>
        /// To get and set FieldValueRules
        /// </summary>
        [XmlArray("MapValues")]
        [XmlArrayItem("MapValue")]
        public MapValue[] MapValues { get; set; }

        /// <summary>
        /// To get and set FieldName
        /// </summary>
        [XmlAttribute("fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// To get DataType
        /// </summary>
        [XmlIgnore]
        public DataType DataType
        {
            get { return _DataType; }
        }

        /// <summary>
        /// To get and set DataTypeString
        /// </summary>
        [XmlAttribute("dataType")]
        public string DataTypeString
        {
            get
            {
                return _DataTypeString;
            }
            set
            {
                _DataTypeString = value;
                SetDataType(value);
            }
        }

        /// <summary>
        /// To get and set ""DataTypeString
        /// </summary>
        [XmlAttribute("dataSourceType")]
        public string DataSourceTypeString
        {
            get
            {
                return _DataSourceTypeString;
            }
            set
            {
                _DataSourceTypeString = value;
                SetDataSourceType(value);
            }
        }

        /// <summary>
        /// To get DataType
        /// </summary>
        [XmlIgnore]
        public DataSourceType DataSourceType
        {
            get { return _DataSourceType; }
        }



        ///// <summary>
        ///// To get and set TargetDataType
        ///// </summary>
        //[XmlAttribute("targetDataType")]
        //public string TargetDataType { get; set; }
        
        #endregion

        # region IDataTransformationTask Methods
        /// <summary>
        /// Transforms the rawData dictionary with XML rules
        /// </summary>
        /// <param name="rawData">Dictionary containing input for DataTransformation</param>
        /// <returns>Transformed dictionary objects</returns>
        Dictionary<string, object> IDataTransformationTask.Transform(Dictionary<string, object> rawData, ref StringBuilder log)
        {
            try
            {
                log.Append("FieldValueMapping transformation started. ");
                if (rawData.ContainsKey(FieldName))
                {
                    if (DataSourceType == DataSourceType.Local)
                    {
                        //Replace the old value with new value                       
                        for (int index = 0; index < MapValues.Length; index++)
                        {
                            if (IsValuesEqual(MapValues[index].OldValue, rawData[FieldName].ToString()))
                            {
                                log.Append(FieldName + ": " + MapValues[index].OldValue + " replaced with " + MapValues[index].NewValue + " | ");
                                rawData[FieldName] = MapValues[index].NewValue;
                                break;
                            }
                        }
                    }
                    else if (DataSourceType == DataSourceType.StoredProcedure)
                    {
                        //To check ProgramId is empty and return 0 if empty and to convert string to int
                        int programId = int.Parse(rawData[FieldName].ToString() == "" ? "0" : rawData[FieldName].ToString());
                        string valueCode = DataTransformationDataService.DataTransformationDAO.GetValueCodeFromProgramId(programId);
                        log.Append(FieldName + ": " + rawData[FieldName] + " replaced with " + valueCode + " | ");
                        rawData[FieldName] = valueCode;
                        
                    }
                    else if (DataSourceType == DataSourceType.WebService)
                    {
                        //Add serrvice reference to the service
                        //Implementation code goes here
                    }
                }
                log.Append("FieldValueMapping transformation end. ");
            }
            catch (Exception ex)
            {
                log.Append(" - FieldValueMapping transformation error  \r\n" + ex.Message + " \r\n");
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return rawData;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Check values are equal or not
        /// </summary>
        /// <param name="fieldToCompare"></param>
        /// <param name="leadDataDictionary"></param>
        /// <returns>return TRUE if success, else FALSE</returns>
        private bool IsValuesEqual(string input1, string input2)
        {
            bool result = false;
            try
            {
                //Converts FieldToCompare to the correct type, and compare
                switch (DataType)
                {
                    case DataType.BooleanData:
                        {
                            return ((string.IsNullOrEmpty(input1) ? false : Convert.ToBoolean(input1)) == (string.IsNullOrEmpty(input2) ? false : Convert.ToBoolean(input2)));
                        }
                    case DataType.StringData:
                        {
                            return (input1.ToLower() == input2.ToLower());
                        }
                    case DataType.IntData:
                        {
                            return ((string.IsNullOrEmpty(input1) ? 0 : Convert.ToInt32(input1)) == (string.IsNullOrEmpty(input2) ? 0 : Convert.ToInt32(input2)));
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return ((string.IsNullOrEmpty(input1) ? 0 : Convert.ToDouble(input1)) == (string.IsNullOrEmpty(input2) ? 0 : Convert.ToDouble(input2)));
                    //    }
                    case DataType.DecimalData:
                        {
                            return ((string.IsNullOrEmpty(input1) ? 0 : Convert.ToDecimal(input1)) == (string.IsNullOrEmpty(input2) ? 0 : Convert.ToDecimal(input2)));
                        }
                    case DataType.Other:
                    case DataType.Unassigned:
                        {                            
                            return false;
                        }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
            return result;
        }

        
        /// <summary>
        /// Set DataSourceType Enum from String
        /// </summary>
        /// <param name="value">value to set</param>
        private void SetDataSourceType(string value)
        {
            try
            {
                if (value == null)
                {
                    _DataSourceType = DataSourceType.Other;
                    return;
                }

                switch (value.ToString().ToLower())
                {
                    case "local":
                        {
                            _DataSourceType = DataSourceType.Local;
                            break;
                        }
                    case "storedprocedure":
                    case "sp":
                    case "storedproc":
                        {
                            _DataSourceType = DataSourceType.StoredProcedure;
                            break;
                        }
                    case "webservice":
                    case "webservices":
                    case "wcfservice":
                    case "wcfservices":
                        {
                            _DataSourceType = DataSourceType.WebService;
                            break;
                        }
                    default:
                        {
                            _DataSourceType = DataSourceType.Other;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
        }

        /// <summary>
        /// Set DataType Enum from String
        /// </summary>
        /// <param name="value">value to set</param>
        private void SetDataType(string value)
        {
            try
            {
                if (value == null)
                {
                    _DataType = DataType.Other;
                    return;
                }

                switch (value.ToString().ToLower())
                {
                    case "stringdata":
                    case "string":
                    case "str":
                        {
                            _DataType = DataType.StringData;
                            break;
                        }
                    case "intdata":
                    case "int":
                    case "integer":
                        {
                            _DataType = DataType.IntData;
                            break;
                        }
                    //case "doubledata":
                    //case "double":
                    //case "dbl":
                    //    {
                    //        _DataType = DataType.DoubleData;
                    //        break;
                    //    }
                    case "decimal":
                        {
                            _DataType = DataType.DecimalData;
                            break;
                        }
                    case "booleandata":
                    case "bool":
                    case "boolean":
                        {
                            _DataType = DataType.BooleanData;
                            break;
                        }
                    case "unassigned":
                        {
                            _DataType = DataType.Unassigned;
                            break;
                        }
                    default:
                        {
                            _DataType = DataType.Other;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                throw;
            }
        }

        #endregion

    }
}
