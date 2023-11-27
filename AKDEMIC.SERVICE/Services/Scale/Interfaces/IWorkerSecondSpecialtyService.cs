using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerSecondSpecialtyService
    {
        Task<WorkerSecondSpecialty> Get(Guid workerSecondSpecialtyId);
        Task<WorkerSecondSpecialty> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerSecondSpecialty>> GetAll();
        Task<List<WorkerSecondSpecialty>> GetAllByUserId(string userId);
        Task Insert(WorkerSecondSpecialty workerSecondSpecialty);
        Task Update(WorkerSecondSpecialty workerSecondSpecialty);
        Task Delete(WorkerSecondSpecialty workerSecondSpecialty);
        Task<int> GetWorkerSecondSpecialtiesQuantity(string userId);
        Task<List<SecondSpecialtiesTemplate>> GetWorkerSecondSpecialtiesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
