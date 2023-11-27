using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public async Task<bool> AnySurvey(int type, string name)
        {
            return await _surveyRepository.AnySurvey(type, name);
        }

        public async Task<bool> AnySurveyByName(int type, int system, string name ,Guid? id = null)
        {
            return await _surveyRepository.AnySurveyByName(type,system, name,id);
        }

        public async Task<Survey> Get(Guid id)
        {
            return await _surveyRepository.Get(id);
        }

        public async Task Insert(Survey survey)
        {
            await _surveyRepository.Insert(survey);
        }

        public async Task<Guid> InsertAndReturnId(Survey survey)
        {
            return await _surveyRepository.InsertAndReturnId(survey);
        }
        

        public async Task Update(Survey survey)
        {
            await _surveyRepository.Update(survey);
        }

        public async Task DeleteById(Guid id)
        {
            await _surveyRepository.DeleteById(id);
        }

        public async Task<SurveyDetailTemplate> GetSurveyDetail(Guid surveyId)
        {
            return await _surveyRepository.GetSurveyDetail(surveyId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            return await _surveyRepository.GetGeneralSurveysDatatable(sentParameters, system, careerId, startSearchDate, endSearchDate, searchValue, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIntranetGeneralSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            return await _surveyRepository.GetIntranetGeneralSurveysDatatable(sentParameters, system, careerId, startSearchDate, endSearchDate, searchValue, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeSurveysDatatable(DataTablesStructs.SentParameters sentParameters, int system, Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            return await _surveyRepository.GetJobExchangeSurveysDatatable(sentParameters, system, careerId, startSearchDate, endSearchDate, searchValue, user);
        }

        public async Task<bool> ValidateSurvey(Guid id)
        {
            return await _surveyRepository.ValidateSurvey(id);
        }

        public async Task<IEnumerable<Survey>> GetSurveisInInterestGroupByUserId(string id)
        {
            return await _surveyRepository.GetSurveisInInterestGroupByUserId(id);
        }

        //public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingSatisfactionSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        //{
        //    return await _surveyRepository.GetTeachingSatisfactionSurveyDatatable(sentParameters, searchValue);
        //}

        public async Task<DataTablesStructs.ReturnedData<object>> GetGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _surveyRepository.GetGeneralTeachingSurveyDatatable(sentParameters, searchValue);
        }

        public async Task<Survey> GetWithIncludes(Guid id)
        {
            return await _surveyRepository.GetWithIncludes(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportGeneralTeachingSurveyDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _surveyRepository.GetReportGeneralTeachingSurveyDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetSurveyByIdSelect2(Select2Structs.RequestParameters requestParameters, Guid eid)
        {
            return await _surveyRepository.GetSurveyByIdSelect2(requestParameters, eid);
        }

        public async Task<bool> ExistSurveyCode(int type, int system,string code, Guid? id = null)
        {
            return await _surveyRepository.ExistSurveyCode(type,system,code, id);
        }

        public async Task<object> GetAllAsSelect2CliendSide(Guid sectionId)
        {
            return await _surveyRepository.GetAllAsSelect2CliendSide(sectionId);
        }

        public Task<IEnumerable<SurveyTemplateA>> GetReportData()
        {
            return _surveyRepository.GetReportData();
        }

        public async Task<List<SurveyReportTemplate>> GetSurveyReportExcel(Guid id)
        {
            return await _surveyRepository.GetSurveyReportExcel(id);
        }
        public Task<List<QuestionExcelTemplate>> GetQuestions(Guid surveyId)
            => _surveyRepository.GetQuestions(surveyId);

        

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system, int type ,string searchValue = null)
            => await _surveyRepository.GetReportSurveyDatatable(sentParameters, system, type, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetIntranetReportSurveyDatatable(DataTablesStructs.SentParameters sentParameters, int system, int type, string searchValue = null)
            => await _surveyRepository.GetIntranetReportSurveyDatatable(sentParameters, system, type, searchValue);

        public async Task<IEnumerable<SurveyTemplateA>> GetReportBySystemAndTypeData(int system, int type)
            => await _surveyRepository.GetReportBySystemAndTypeData(system,type);

        public async Task<object> GetSurveyByCareerIdSelect2ClientSide(Guid careerId, int? surveyType = null, int? system = null, int? year = null)
        {
            return await _surveyRepository.GetSurveyByCareerIdSelect2ClientSide(careerId, surveyType, system, year);
        }

        public async Task<object> GetPreEnrollmentSurveySelect2ClientSide()
            => await _surveyRepository.GetPreEnrollmentSurveySelect2ClientSide();

        public Task<decimal> GetSurveyProgressPercentage(Guid surveyId)
            => _surveyRepository.GetSurveyProgressPercentage(surveyId);

        public Task<Survey> GetByCode(string code, int system)
            => _surveyRepository.GetByCode(code, system);

        public Task<bool> HasSurveyItems(Guid surveyId)
            => _surveyRepository.HasSurveyItems(surveyId);
    }
}