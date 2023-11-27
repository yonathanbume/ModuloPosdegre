using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IEmployeeSurveyService
    {
        Task<IEnumerable<EmployeeSurveyTemplate>> GetAllTemplateByUser(string userId, string companyId);
        Task Insert(EmployeeSurvey employeeSurvey);
        Task <bool> StudentSurveyed(Guid surveyId, string companyUserId, string studentUserId);
        Task<DataTablesStructs.ReturnedData<object>> GetUserEmployeeSurveyDatatable(DataTablesStructs.SentParameters sentParameters, bool graduated, Guid surveyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEmployeeSurveyByCompany(DataTablesStructs.SentParameters sentParameters ,string userId ,string searchValue = null);
    }
}
