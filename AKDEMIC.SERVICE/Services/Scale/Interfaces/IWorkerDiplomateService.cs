using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerDiplomateService
    {
        Task<WorkerDiplomate> Get(Guid workerDiplomateId);
        Task<WorkerDiplomate> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerDiplomate>> GetAll();
        Task<List<WorkerDiplomate>> GetAllByUserId(string userId);
        Task Insert(WorkerDiplomate workerDiplomate);
        Task Update(WorkerDiplomate workerDiplomate);
        Task Delete(WorkerDiplomate workerDiplomate);
        Task<int> GetWorkerDiplomatesQuantity(string userId);
        Task<List<DiplomatesTemplate>> GetWorkerDiplomatesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
