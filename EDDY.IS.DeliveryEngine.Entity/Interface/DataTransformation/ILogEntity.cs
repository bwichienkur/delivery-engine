namespace EDDY.IS.DeliveryEngine.Entity.DataTransformation.Interface
{
    public interface ILogEntity
    {
        #region Properties
        int Id { get; set; }
        int? LeadId { get; set; }
        int? RowLeadId { get; set; }
        string Message { get; set; }    
        #endregion
    }
}
