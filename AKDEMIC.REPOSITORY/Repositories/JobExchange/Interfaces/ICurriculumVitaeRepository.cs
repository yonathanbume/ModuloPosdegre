using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ICurriculumVitaeRepository:IRepository<CurriculumVitae>
    {
        Task<CurriculumVitae> GetByStudent(Guid studentId);
        Task<CurriculumVitaeTemplate> GetCurriculumVitaeByStudentId(Guid studentId);
        Task<CurriculumVitaeTemplate> GetCurriculumVitae(Guid id);
    }
}
