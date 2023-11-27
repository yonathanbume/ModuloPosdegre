using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerOtherStudyService
    {
        Task<WorkerOtherStudy> Get(Guid workerOtherStudyId);
        Task<WorkerOtherStudy> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerOtherStudy>> GetAll();
        Task<List<WorkerOtherStudy>> GetAllByUserId(string userId);
        Task Insert(WorkerOtherStudy workerOtherStudy);
        Task Update(WorkerOtherStudy workerOtherStudy);
        Task Delete(WorkerOtherStudy workerOtherStudy);
        Task<int> GetWorkerOtherStudiesQuantity(string userId);
        Task<List<OtherStudiesTemplate>> GetWorkerOtherStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
