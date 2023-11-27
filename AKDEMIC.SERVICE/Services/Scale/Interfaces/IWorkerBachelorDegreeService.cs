using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerBachelorDegreeService
    {
        Task<WorkerBachelorDegree> Get(Guid workerBachelorDegreeId);
        Task<WorkerBachelorDegree> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerBachelorDegree>> GetAll();
        Task<List<WorkerBachelorDegree>> GetAllByUserId(string userId);
        Task Insert(WorkerBachelorDegree workerBachelorDegree);
        Task Update(WorkerBachelorDegree workerBachelorDegree);
        Task Delete(WorkerBachelorDegree workerBachelorDegree);
        Task<int> GetWorkerBachelorDegreesQuantity(string userId);
        Task<List<BachelorDegreesTemplate>> GetWorkerBachelorDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
