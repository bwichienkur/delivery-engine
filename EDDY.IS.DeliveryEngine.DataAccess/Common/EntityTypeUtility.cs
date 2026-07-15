using System;
using System.Data;
//using EDDY.Nexus.Common.ExceptionHandler;
using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;


namespace EDDY.IS.DeliveryEngine.DataAccess.Common
{
    public class EntityTypeUtility
    {
        # region Public Method
        /// <summary>
        /// Returns DbType enum based on DataType input
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns>returns DbType</returns>
        public static DbType getDBTypeFromEntityDataType(string dataType)
        {
            try
            {
                switch (dataType.ToLower())
                {
                    case "bool":
                    case "boolean":
                        {
                            return DbType.Boolean;
                        }
                    case "string":
                    case "varchar":
                        {
                            return DbType.String;
                        }
                    case "int":
                    case "integer":
                        {
                            return DbType.Int32;
                        }
                    //case "double":
                    //    {
                    //        return DbType.Double;
                    //    }
                    case "decimal":
                        {
                            return DbType.Decimal;
                        }
                    default:
                        {
                            return DbType.String;
                        }
                }
            }
            catch (Exception ex)
            {
                ////ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                //return DbType.String;
                throw;
            }
        }

        /// <summary>
        /// Returns Object based on value string and type in given FTC parameter
        /// </summary>
        /// <param name="ftcParameter">FTC Parameter to use</param>
        /// <returns>returns Object</returns>
        public static object getObjectFromFTCParameter(FtcParameter ftcParameter)
        {
            try
            {
                switch (ftcParameter.DataType.ToLower())
                {
                    case "bool":
                    case "boolean":
                        {
                            return Convert.ToBoolean(ftcParameter.Value);
                        }
                    case "string":
                    case "varchar":
                        {
                            return (ftcParameter.Value);
                        }
                    case "int":
                    case "integer":
                        {
                            return Convert.ToInt32(ftcParameter.Value);
                        }
                    //case "double":
                    //    {
                    //        return Convert.ToDouble(ftcParameter.Value);
                    //    }
                    case "decimal":
                        {
                            return Convert.ToDecimal(ftcParameter.Value);
                        }
                    default:
                        {
                            return DbType.String;
                        }
                }
            }
            catch (Exception ex)
            {
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                //return DbType.String;
                throw;
            }
        }

        /// <summary>
        /// Returns datatype based on DataType input
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns>returns Type</returns>
        public static Type getTypeFromEntityDataType(DataType dataType)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.BooleanData:
                        {
                            return typeof(bool);
                        }
                    case DataType.StringData:
                        {
                            return typeof(string);
                        }
                    case DataType.IntData:
                        {
                            return typeof(Int32);
                        }
                    //case DataType.DoubleData:
                    //    {
                    //        return typeof(double);
                    //    }
                    case DataType.DecimalData:
                        {
                            return typeof(Decimal);
                        }
                    default:
                        {
                            return null;
                        }
                }
            }
            catch (Exception ex)
            {
                //ExceptionWrapper.ExceptionHandler.HandleException(ex, Policies.DATA_ACCESS_POLICY);
                //return null;
                throw;
            }

        }
        
        #endregion
    }
}
