using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerSecondSpecialtyRepository : IRepository<WorkerSecondSpecialty>
    {
        Task<List<WorkerSecondSpecialty>> GetAllByUserId(string userId);
        Task<WorkerSecondSpecialty> GetWithIncludes(Guid id);
        Task<int> GetWorkerSecondSpecialtiesQuantity(string userId);
        Task<List<SecondSpecialtiesTemplate>> GetWorkerSecondSpecialtiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
