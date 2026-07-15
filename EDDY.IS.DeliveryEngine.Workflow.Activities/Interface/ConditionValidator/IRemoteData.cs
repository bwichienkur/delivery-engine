//using EDDY.Nexus.Entity.ConditionValidator;

using EDDY.IS.DeliveryEngine.Entity.ConditionValidator;
namespace EDDY.IS.DeliveryEngine.Workflow.Activities.ConditionValidator.Interface
{

    public interface IRemoteData
    {
        #region Properties
        
        FtcParameter[] Parameters { get; set; }  //collection of parameters for the sp/ws/class
        DataSourceType RemoteDataType { get; set; } //storedProc, webService or Class?
        DataType ReturnDataType { get; set; } //type of data being returned

        #endregion

        #region Public Methods

        object GetData();  //Returns a list of objects for condtion comparison

        #endregion
    }

}