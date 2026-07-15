using System;
using System.Xml.Serialization;
using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;


namespace EDDY.IS.DeliveryEngine.DataAccess.Common.ConditionValidator
{
    public class FieldToCompare
    {
        # region Private Variables
        private RelOperator _relOperator;
        private string _relOperatorString;
        private DataSourceType _dataSourceType;
        private string _dataSourceTypeString;
        private DataType _dataType;
        private string _dataTypeString;
        #endregion

        # region Constructor

        #endregion

        # region Properties

        /// <summary>
        /// To get and set Id
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// To get and set FieldName
        /// </summary>
        [XmlAttribute("fieldName")]
        public string FieldName { get; set; }


        /// <summary>
        /// To get and set DataTypeString
        /// </summary>
        [XmlAttribute("dataType")]
        public string DataTypeString
        {
            get
            {
                return _dataTypeString;
            }
            set
            {
                _dataTypeString = value;
                SetDataType(value);
            }
        }

        /// <summary>
        /// To get and set RelOperatorString
        /// </summary>
        [XmlAttribute("expression")]
        public string RelOperatorString
        {
            get
            {
                return _relOperatorString;
            }
            set
            {
                _relOperatorString = value;
                SetRelOperator(value);
            }
        }

        /// <summary>
        /// To get DataSourceType
        /// </summary>
        [XmlIgnore]
        public DataSourceType DataSourceType
        {
            get { return _dataSourceType; }
        }

        /// <summary>
        /// To get and set DataSourceTypeString
        /// </summary>
        [XmlAttribute("dataSourceType")]
        public string DataSourceTypeString
        {
            get { return _dataSourceTypeString; }
            set
            {
                _dataSourceTypeString = value;
                SetDataSourceType(value);
            }
        }

        /// <summary>
        /// To get DataType
        /// </summary>
        [XmlIgnore]
        public DataType DataType
        {
            get { return _dataType; }
        }

        /// <summary>
        /// To get HasMultipleDataVals
        /// </summary>
        [XmlIgnore]
        public bool HasMultipleDataVals
        {
            get
            {
                return (Data.Contains("|") ||
                          (Data.Contains(",") && !Data.Contains("|")));
            }
        }

        /// <summary>
        /// To get RelOperator
        /// </summary>
        [XmlIgnore]
        public RelOperator RelOperator
        {
            get { return _relOperator; }
        }

        /// <summary>
        /// To get and set Data
        /// </summary>
        [XmlAttribute("data")]
        public string Data { get; set; }

        /// <summary>
        /// To get and set FTCParameters
        /// </summary>
        [XmlArray("FTCParameters")]
        [XmlArrayItem("FTCParameter")]
        public FtcParameter[] FTCParameters { get; set; }
        
        #endregion
        
        # region Private Methods                
        
        /// <summary>
        /// Set DataSourceType Enum from String
        /// </summary>
        /// <param name="value">value to set</param> 
        private void SetDataSourceType(string value)
        {
            try
            {
                switch (value.ToLower())
                {
                    case "local":
                        {
                            _dataSourceType = DataSourceType.Local;
                            break;
                        }
                    case "storedprocedure":
                        {
                            _dataSourceType = DataSourceType.StoredProcedure;
                            break;
                        }
                    case "webservice":
                        {
                            _dataSourceType = DataSourceType.Webservice;
                            break;
                        }
                    case "class":
                        {
                            _dataSourceType = DataSourceType.Class;
                            break;
                        }
                    default:
                        {
                            _dataSourceType = DataSourceType.Other;
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
                    _dataType = DataType.Other;
                    return;
                }

                switch (value.ToLower())
                {
                    case "stringdata":
                    case "string":
                    case "str":
                        {
                            _dataType = DataType.StringData;
                            break;
                        }
                    case "intdata":
                    case "int":
                    case "integer":
                        {
                            _dataType = DataType.IntData;
                            break;
                        }
                    case "decimal":
                        {
                            _dataType = DataType.DecimalData;
                            break;
                        }
                    case "booleandata":
                    case "bool":
                    case "boolean":
                        {
                            _dataType = DataType.BooleanData;
                            break;
                        }
                    case "unassigned":
                        {
                            _dataType = DataType.Unassigned;
                            break;
                        }
                    default:
                        {
                            _dataType = DataType.Other;
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
        /// Set RelOperator Enum from String
        /// </summary>
        /// <param name="value">value to set</param>        
        private void SetRelOperator(string value)
        {
            try
            {
                switch (value.ToLower())
                {
                    case "equal":
                    case "eq":
                    case "equalto":
                    case "equals":
                        {
                            _relOperator = RelOperator.Equals;
                            break;
                        }
                    case "in":
                    case "is":
                        {
                            _relOperator = RelOperator.In;
                            break;
                        }
                    case "lessthan":
                    case "lt":
                        {
                            _relOperator = RelOperator.LessThan;
                            break;
                        }
                    case "lessthanorequalto":
                    case "lteq":
                    case "lte":
                        {
                            _relOperator = RelOperator.LessThanOrEqualTo;
                            break;
                        }
                     case "greaterthan":
                     case "gt":
                        {
                            _relOperator = RelOperator.GreaterThan;
                            break;
                        }
                     case "greaterthanorequalto":
                     case "gteq":
                     case "gte":
                        {
                            _relOperator = RelOperator.GreaterThanOrEqualTo;
                            break;
                        }
                    default:
                        {
                            _relOperator = RelOperator.Other;
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

