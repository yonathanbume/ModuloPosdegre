using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerProfessionalSchoolRepository : IRepository<WorkerProfessionalSchool>
    {
        Task<List<WorkerProfessionalSchool>> GetAllByUserId(string userId);
        Task<WorkerProfessionalSchool> GetWithIncludes(Guid id);
        Task<int> GetWorkerProfessionalSchoolsQuantity(string userId);
        Task<List<ProfesionalSchoolTemplate>> GetWorkerProfessionalSchoolsByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
