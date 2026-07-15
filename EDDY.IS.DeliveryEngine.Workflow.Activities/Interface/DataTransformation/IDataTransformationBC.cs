using System.Data;
using System.Collections.Generic;
using System.Text;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.DataTransformation;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.DataTransformation.Interface
{
    public interface IDataTransformationBC
    {
        # region Public Methods
        //List<KeyValueObject> ConvertDictionaryToKeyValueObject(Dictionary<string, object> dicValue); //Converts the dictionary object to Key/Value object
        //Dictionary<string, object> ConvertKeyValueToDictionary(List<KeyValueObject> keyValue); //Converts the Key/Value object to a dictionary objects        
        //Dictionary<string, object> ConvertKeyValueToDictionary(string leadDataString, char seperator); //Converts the Key/Value string to a dictionary objects        
        //bool CreateData(DataTransformationEntity data); //Creates a record in Database 
        //bool CreateLog(int? leadId, int? rowLeadId, StringBuilder message, int createBy); //To enter log data to table
        //bool DeleteData(int Id); //Removes data from Database for the given id       
        //List<DataTransformationEntity> GetAllData(); //Gets DataTransformaiton data from SQL server
        //DataTransformationEntity GetData(int id); //Get data for the id 
        //List<DeliveryDefsEntity> GetDeliveryDefs(); //To get list of delivery defs
        //Dictionary<string, string> GetFieldNames(); //To get field names and populate in admin 
        //List<FormValidationEntity> GetFormValidations(); //To get list of form validations
        //string GetNameValueString(Dictionary<string, object> nameVals, char seperator); //Converts the input dictionary to Key/Value string by splitting based on seperator
        //List<DataTransformationEntity> GetTasksByConditionId(int id); //To get task data based on condition id
        //List<DataTransformationEntity> GetTasksByDeliveryDefId(int id); //To get task data based on delivery def id
        //List<DataTransformationEntity> GetTasksByFormValidationId(int id); //To get task data based on form validation id
        //string GetXSLById(int Id); //To get XSL string for the given id
        Dictionary<string, object> Transform(Dictionary<string, object> rawData, Dictionary<string, object> leadData, EventType eventType, int eventId, ref StringBuilder log); //Transforms the leadData dictionary with XML rules        
        //bool UpdateConditionXml(ConditionXmlEntity data); //Updates condition data into Database
        //bool UpdateData(DataTransformationEntity data); //Saves data into Database        
        //bool UpdateTaskXml(TaskXmlEntity data); //Updates task data into Database
        //bool UpdateSequenceNo(string data); //To update sequence number
        //DataTable GetELRNexusMappingData(DataTable datatable);

        #endregion
        #region Added By Bala 08/19/2011
        //DataTransformationEntityNew GetDataTransformationTask(DTConditionparamEntity cp);

        //ConditionSelectEntity GetDataTransformationCondition(DTConditionparamEntity ce);

        //bool RemoveDataTransformationCondition(DTConditionparamEntity ce);

        //DTConditionparamEntity AddUpdateDataTransformationCondition(DTConditionparamEntity ce);

        //DTConditionparamEntity AddUpdateDataTransformationTasks(DTConditionparamEntity ce);

        //List<AppendNameValueTaskItem> GetAppendNameValueTasks(DTConditionparamEntity ce);

        //TaskSelectEntity GetDataTransformationAppendNameValueTask(DTConditionparamEntity ce);

        //bool DTCheckDuplicateSequence(DTConditionparamEntity ce);

        //List<AppendNameValueTaskItem> GetFieldFormatTasks(DTConditionparamEntity ce);

        //Dictionary<string, string> GetDataTranformationFormats(string DateOrPhone);
        //List<AppendNameValueTaskItem> GetXsltXmlTasks(DTConditionparamEntity ce);

        //DTConditionparamEntity AddUpdateXsltXmlTasks(DTConditionparamEntity ce);


        //XsltXmlEntity GetXsltXmlTask(DTConditionparamEntity ce);

        //List<AppendNameValueTaskItem> GetEmailSubjectAndEmailToTasks(DTConditionparamEntity ce);

        #endregion
    }
}
