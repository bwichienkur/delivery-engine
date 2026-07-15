namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    #region Enum
    /// <summary>
    /// To populate event type
    /// </summary>
    /*public enum EventType : int
    {
        Validation = 1,
        Database,
        Delivery
    }*/

    /// <summary>
    /// To populate DataType
    /// </summary>
    public enum DataType
    {
        StringData, IntData, DecimalData, BooleanData, Unassigned, Other
    }

    /// <summary>
    /// To populate DataSourceType
    /// </summary>
    public enum DataSourceType
    {
        Local, StoredProcedure, WebService, Other
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum ConditionXMLTableField
    {
        ConditionXMLId,
        ConditionXML,
        ConditionDescription,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum LogTableField
    {
        Id,
        LeadId,
        RowLeadId,
        LogMessage,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum TaskTableField
    {
        Id,
        Name,
        ConditionXmlId,
        ConditionXml,
        DeliveryDefId,
        FormValidationId,
        TaskXmlId,
        TaskXml,
        XSL,
        SequenceNo,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum TaskXMLTableField
    {
        TaskXMLId,
        TaskXML,
        TaskDescription,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum DeliveryDefTableField
    {
        DeliveryDefId,
        Description,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }

    /// <summary>
    /// This enum documents the order in which data is arranged in the database table.
    /// </summary>
    public enum FormValidationTableField
    {
        FormValidationId,
        Description,
        IsEnabled,
        CreatedBy,
        CreateDate,
        UpdateDate,
        UpdatedBy,
        RowGuid
    }   


    #endregion
}
