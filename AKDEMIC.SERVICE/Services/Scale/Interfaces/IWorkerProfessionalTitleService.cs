using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerProfessionalTitleService
    {
        Task<WorkerProfessionalTitle> Get(Guid workerProfessionalTitleId);
        Task<WorkerProfessionalTitle> GetWithIncludes(Guid id);
        Task<IEnumerable<WorkerProfessionalTitle>> GetAll();
        Task<List<WorkerProfessionalTitle>> GetAllByUserId(string userId);
        Task Insert(WorkerProfessionalTitle workerProfessionalTitle);
        Task Update(WorkerProfessionalTitle workerProfessionalTitle);
        Task Delete(WorkerProfessionalTitle workerProfessionalTitle);
        Task<int> GetWorkerProfessionalTitlesQuantity(string userId);
        Task<List<ProfessionalTitlesTemplate>> GetWorkerProfessionalTitlesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
