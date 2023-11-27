using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerTechnicalStudyRepository : IRepository<WorkerTechnicalStudy>
    {
        Task<List<WorkerTechnicalStudy>> GetAllByUserId(string userId);
        Task<WorkerTechnicalStudy> GetWithIncludes(Guid id);
        Task<int> GetWorkerTechnicalStudiesQuantity(string userId);
        Task<List<TechnicalStudiesTemplate>> GetWorkerTechnicalStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
