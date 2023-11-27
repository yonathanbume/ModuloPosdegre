using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerMasterDegreeRepository : IRepository<WorkerMasterDegree>
    {
        Task<List<WorkerMasterDegree>> GetAllByUserId(string userId);
        Task<WorkerMasterDegree> GetWithIncludes(Guid id);
        Task<int> GetWorkerMasterDegreesQuantity(string userId);
        Task<List<MasterDegreesTemplate>> GetWorkerMasterDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
