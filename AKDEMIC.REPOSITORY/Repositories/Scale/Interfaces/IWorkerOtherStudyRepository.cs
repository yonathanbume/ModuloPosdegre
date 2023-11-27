using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerOtherStudyRepository : IRepository<WorkerOtherStudy>
    {
        Task<List<WorkerOtherStudy>> GetAllByUserId(string userId);
        Task<WorkerOtherStudy> GetWithIncludes(Guid id);
        Task<int> GetWorkerOtherStudiesQuantity(string userId);
        Task<List<OtherStudiesTemplate>> GetWorkerOtherStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
