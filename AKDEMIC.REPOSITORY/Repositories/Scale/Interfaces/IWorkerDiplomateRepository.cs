using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerDiplomateRepository : IRepository<WorkerDiplomate>
    {
        Task<List<WorkerDiplomate>> GetAllByUserId(string userId);
        Task<WorkerDiplomate> GetWithIncludes(Guid id);
        Task<int> GetWorkerDiplomatesQuantity(string userId);
        Task<List<DiplomatesTemplate>> GetWorkerDiplomatesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
