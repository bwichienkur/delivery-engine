
namespace EDDY.IS.DeliveryEngine.Entity.ConditionValidator
{
    #region Enum
    ///// <summary>
    ///// Event for which the condition will be applied
    ///// </summary>
    //public enum EventType
    //{
    //    Validation = 1, Database, Delivery
    //}

    /// <summary>
    /// Data source type for comparison data
    /// </summary>
    public enum DataSourceType
    {
        Local, StoredProcedure, Webservice, Class, Other
    }

    /// <summary>
    /// Data type of comparison variable
    /// </summary>
    public enum DataType
    {
        StringData, IntData, DecimalData, BooleanData, Unassigned, Other
    }

    /// <summary>
    /// Relational operator for condition comparison
    /// </summary>
    public enum RelOperator
    {
        Equals, In, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo, RegularExpression, Other
    }
    #endregion


    
}
