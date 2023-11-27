using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerProfessionalTitleRepository : IRepository<WorkerProfessionalTitle>
    {
        Task<List<WorkerProfessionalTitle>> GetAllByUserId(string userId);
        Task<WorkerProfessionalTitle> GetWithIncludes(Guid id);
        Task<int> GetWorkerProfessionalTitlesQuantity(string userId);
        Task<List<ProfessionalTitlesTemplate>> GetWorkerProfessionalTitlesByPaginationParameters(string userId, PaginationParameter paginationParameter);
    }
}
