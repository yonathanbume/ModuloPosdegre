using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ISurveyService
    {
        Task<Survey> Get(Guid id);
        Task<Survey> GetByCode(string code, int system);
        Task<Survey> GetWithIncludes(Guid id);
        Task Insert(Survey survey);
        Task<Guid> InsertAndReturnId(Survey survey);
        Task Update(Survey survey);
        Task DeleteById(Guid id);
        Task<bool> ValidateSurvey(Guid id);
        Task<bool> HasSurveyItems(Guid surveyId);
        Task<bool> AnySurvey(int type, string name);
        Task<bool> AnySurveyByName(int type, int system, string name, Guid? id = null);
        Task<SurveyDetailTemplate> GetSurveyDetail(Guid surveyId);
        Task<DataTablesStructs.ReturnedData<object>> GetReportGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system , int type,string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIntranetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system, int type, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIntranetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null);        
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null);
        //Task<DataTablesStructs.ReturnedData<object>> GetTeachingSatisfactionSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<Survey>> GetSurveisInInterestGroupByUserId(string id);
        Task<decimal> GetSurveyProgressPercentage(Guid surveyId);
        Task<Select2Structs.ResponseParameters> GetSurveyByIdSelect2(Select2Structs.RequestParameters requestParameters, Guid eid);
        Task<bool> ExistSurveyCode(int type,int system,string code, Guid? id = null);
        Task<object> GetAllAsSelect2CliendSide(Guid sectionId);
        Task<List<QuestionExcelTemplate>> GetQuestions(Guid surveyId);
        Task<List<SurveyReportTemplate>> GetSurveyReportExcel(Guid id);
        Task<IEnumerable<SurveyTemplateA>> GetReportData();
        Task<object> GetSurveyByCareerIdSelect2ClientSide(Guid careerId,int? surveyType = null, int? system = null, int? year = null);
        Task<IEnumerable<SurveyTemplateA>> GetReportBySystemAndTypeData(int system,int type);
        Task<object> GetPreEnrollmentSurveySelect2ClientSide();
    }
}