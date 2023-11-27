using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentExperienceRepository : IRepository<StudentExperience>
    {
        Task<StudentExperience> GetLastByStartDate();        
        Task<List<ExperienceDate>> GetAllByStudentTemplate(Guid studentId);
        Task<object> GetStudentExperiencesByStudent(Guid studentId);
        Task<StudentExperience> FirstOrDefaultById(Guid id);
        Task<object> GetStudentExperiencesById(Guid id);
        Task<object> GetStudentExperiencePersonalized(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentWorkingDatatable(DataTablesStructs.SentParameters sentParameters, Guid? companyId, Guid? careerId, string searchValue = null);
        Task<bool> ExistStudentExperienceByCompany(Guid companyId, Guid StudentId);
        Task<StudentExperience> GetYearWorking();
        Task<object> GetStudentExperiencesByCompanyIdSelect2ClientSide(Guid companyId);
        Task<object> GetWorkingBachelorsChart(string startSearchDate = null, string endSearchDate = null);
        Task<object> GetJobExchangeStudentExperienceCareerReportChart(List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null);
        Task<object> GetJobExchangeStudentExperienceSectorReportChart();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceSectorReportDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
