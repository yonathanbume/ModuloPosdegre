using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerBachelorDegreeRepository : IRepository<WorkerBachelorDegree>
    {
        Task<List<WorkerBachelorDegree>> GetAllByUserId(string userId);
        Task<WorkerBachelorDegree> GetWithIncludes(Guid id);
        Task<int> GetWorkerBachelorDegreesQuantity(string userId);
        Task<List<BachelorDegreesTemplate>> GetWorkerBachelorDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
