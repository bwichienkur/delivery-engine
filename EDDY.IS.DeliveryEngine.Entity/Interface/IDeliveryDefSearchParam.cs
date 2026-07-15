namespace EDDY.IS.DeliveryEngine.Entity.Interface
{
    public interface IDeliveryDefSearchParam
    {
        int StartPosition { get; set; }
        int EndPosition { get; set; }
        string SortField { get; set; }
        int Pagesize { get; set; }
        int CrId { get; set; }
        int DeliveryDefId { get; set;}
        string NameStartsWith { get; set; }
        string NameContains { get; set; }
    }
}
