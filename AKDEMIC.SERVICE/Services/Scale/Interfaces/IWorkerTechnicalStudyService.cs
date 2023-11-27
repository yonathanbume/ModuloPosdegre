using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerTechnicalStudyService
    {
        Task<WorkerTechnicalStudy> Get(Guid workerTechnicalStudyId);
        Task<WorkerTechnicalStudy> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerTechnicalStudy>> GetAll();
        Task<List<WorkerTechnicalStudy>> GetAllByUserId(string userId);
        Task Insert(WorkerTechnicalStudy workerTechnicalStudy);
        Task Update(WorkerTechnicalStudy workerTechnicalStudy);
        Task Delete(WorkerTechnicalStudy workerTechnicalStudy);
        Task<int> GetWorkerTechnicalStudiesQuantity(string userId);
        Task<List<TechnicalStudiesTemplate>> GetWorkerTechnicalStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
