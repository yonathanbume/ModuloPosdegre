using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringCoordinatorRepository : IRepository<TutoringCoordinator>
    {
        Task<TutoringCoordinator> GetWithUserById(string tutoringCoordinatorId);
        Task<DataTablesStructs.ReturnedData<TutoringCoordinator>> GetTutoringCoordinatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null);
        Task<TutoringCoordinator> GetByCareerId(Guid careerId);
        SelectList GetTypeTime();
        Task<TutoringCoordinator> GetWithData(string id);
        Task<bool> AnyByCareerId(Guid careerId);
        Task<string> GetCareerByUserId(string userId);
    }
}
