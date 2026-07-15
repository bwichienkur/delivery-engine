using System;
using System.Data;
using System.Collections.Generic;
//using EDDY.Nexus.DataAccess.Interface.Common;
//using EDDY.Nexus.Entity.DataTransformation;
using EDDY.IS.DeliveryEngine.DataAccess.Interface.Common;
using EDDY.IS.DeliveryEngine.Entity.DataTransformation;
using EDDY.IS.DeliveryEngine.Entity.Common;

namespace EDDY.IS.DeliveryEngine.DataAccess.Interface
{
    public interface IDataTransformationDAO : IBaseDataSource
    {
        # region Public Methods
        bool CreateData(DataTransformationEntity data); //Creates a record in Database 
        bool CreateLog(LogEntity data); //To enter log data to table
        bool DeleteData(int Id); //Removes data from Database for the given id
        List<DataTransformationEntity> GetAllData(); //Gets DataTransformaiton data from SQL server
        DataTransformationEntity GetData(int Id); //Get data for the id
        List<DeliveryDefsEntity> GetDeliveryDefs(); //To get list of delivery defs
        Dictionary<string, string> GetFieldNames(); //To get field names and populate in admin 
        List<FormValidationEntity> GetFormValidations(); //To get list of form validations
        List<DataTransformationEntity> GetTasksByConditionId(int id); //To get task data based on condition id
        List<DataTransformationEntity> GetTasksByDeliveryDefId(int id); //To get task data based on delivery def id
        List<DataTransformationEntity> GetTasksByFormValidationId(int id); //To get task data based on form validation id
        string GetValueCodeFromProgramId(int ProgramId); // To get value code based on program id
        string GetXSLById(int id); //To get XSL string for the given id
        DataTable LoadDataTransformationTable(EventType eventType, int eventId); //Gets DataTransformaiton data from SQL server
        bool UpdateConditionXml(ConditionXmlEntity data); //Updates condition data into Database
        bool UpdateData(DataTransformationEntity data); //Saves data into Database        
        bool UpdateTaskXml(TaskXmlEntity data); //Updates task data into Database
        bool UpdateSequenceNo(string data); //To update sequence number
        DataTable GetELRNexusMappingData(DataTable table);//To give the mapping data for Nexus
        List<ConsumerIdMapping> GetConsumerIdMapping(DataTable table, long serviceTransactionId);
       
        # endregion
        #region Added By Bala on 08/19/2011
        DataTransformationEntityNew GetDataTransformationTask(DTConditionparamEntity cp);

        ConditionSelectEntity GetDataTransformationCondition(DTConditionparamEntity ce);

        bool RemoveDataTransformationCondition(DTConditionparamEntity ce);

        DTConditionparamEntity AddUpdateDataTransformationCondition(DTConditionparamEntity ce);

        DTConditionparamEntity AddUpdateDataTransformationTasks(DTConditionparamEntity ce);

        List<AppendNameValueTaskItem> GetAppendNameValueTasks(DTConditionparamEntity ce);

        TaskSelectEntity GetDataTransformationAppendNameValueTask(DTConditionparamEntity ce);
        bool DTCheckDuplicateSequence(DTConditionparamEntity ce);

        List<AppendNameValueTaskItem> GetFieldFormatTasks(DTConditionparamEntity ce);

        Dictionary<string, string> GetDataTranformationFormats(string DateOrPhone);
        List<AppendNameValueTaskItem> GetXsltXmlTasks(DTConditionparamEntity ce);

        DTConditionparamEntity AddUpdateXsltXmlTasks(DTConditionparamEntity ce);

        XsltXmlEntity GetXsltXmlTask(DTConditionparamEntity ce);

        List<AppendNameValueTaskItem> GetEmailSubjectAndEmailToTasks(DTConditionparamEntity ce);
        #endregion

        string GetStoredProcResult(string procedureName, Int64 leadId);
    }
}
