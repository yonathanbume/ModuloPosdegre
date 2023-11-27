using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerDoctoralDegreeService
    {
        Task<WorkerDoctoralDegree> Get(Guid workerDoctoralDegreeId);
        Task<WorkerDoctoralDegree> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerDoctoralDegree>> GetAll();
        Task<List<WorkerDoctoralDegree>> GetAllByUserId(string userId);
        Task Insert(WorkerDoctoralDegree workerDoctoralDegree);
        Task Update(WorkerDoctoralDegree workerDoctoralDegree);
        Task Delete(WorkerDoctoralDegree workerDoctoralDegree);
        Task<int> GetWorkerDoctoralDegreesQuantity(string userId);
        Task<List<DoctoralDegreesTemplate>> GetWorkerDoctoralDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
