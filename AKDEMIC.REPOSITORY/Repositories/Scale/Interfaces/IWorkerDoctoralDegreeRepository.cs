using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerDoctoralDegreeRepository : IRepository<WorkerDoctoralDegree>
    {
        Task<List<WorkerDoctoralDegree>> GetAllByUserId(string userId);
        Task<WorkerDoctoralDegree> GetWithIncludes(Guid id);
        Task<int> GetWorkerDoctoralDegreesQuantity(string userId);
        Task<List<DoctoralDegreesTemplate>> GetWorkerDoctoralDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
