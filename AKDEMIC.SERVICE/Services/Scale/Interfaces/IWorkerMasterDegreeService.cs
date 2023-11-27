using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerMasterDegreeService
    {
        Task<WorkerMasterDegree> Get(Guid workerMasterDegreeId);
        Task<WorkerMasterDegree> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerMasterDegree>> GetAll();
        Task<List<WorkerMasterDegree>> GetAllByUserId(string userId);
        Task Insert(WorkerMasterDegree workerMasterDegree);
        Task Update(WorkerMasterDegree workerMasterDegree);
        Task Delete(WorkerMasterDegree workerMasterDegree);
        Task<int> GetWorkerMasterDegreesQuantity(string userId);
        Task<List<MasterDegreesTemplate>> GetWorkerMasterDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
