using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class EmployeeSurveyService : IEmployeeSurveyService
    {
        private readonly IEmployeeSurveyRepository _employeeSurveyRepository;

        public EmployeeSurveyService(IEmployeeSurveyRepository employeeSurveyRepository)
        {
            _employeeSurveyRepository = employeeSurveyRepository;
        }

        public async Task Insert(EmployeeSurvey employeeSurvey)
        {
            await _employeeSurveyRepository.Insert(employeeSurvey);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserEmployeeSurveyDatatable(DataTablesStructs.SentParameters sentParameters, bool graduated, Guid surveyId, string searchValue = null)
        {
            return await _employeeSurveyRepository.GetUserEmployeeSurveyDatatable(sentParameters, graduated, surveyId , searchValue);
        }

        public async Task<IEnumerable<EmployeeSurveyTemplate>> GetAllTemplateByUser(string userId, string companyId)
        {
            return await _employeeSurveyRepository.GetAllTemplateByUser(userId, companyId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEmployeeSurveyByCompany(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _employeeSurveyRepository.GetEmployeeSurveyByCompany(sentParameters, userId, searchValue);
        }

        public Task<bool> StudentSurveyed(Guid surveyId, string companyUserId, string studentUserId)
            => _employeeSurveyRepository.StudentSurveyed(surveyId, companyUserId, studentUserId);
    }
}
