using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ICurriculumVitaeService
    {
        Task<CurriculumVitae> GetByStudent(Guid studentId);
        Task Insert(CurriculumVitae curriculumVitae);
        Task Update(CurriculumVitae curriculumVitae);
        Task<CurriculumVitae> Get(Guid id);
        Task<CurriculumVitaeTemplate> GetCurriculumVitaeByStudentId(Guid studentId);
        Task<CurriculumVitaeTemplate> GetCurriculumVitae(Guid id);
    }
}
