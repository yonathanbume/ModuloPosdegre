using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringCoordinatorService
    {
        Task<DataTablesStructs.ReturnedData<TutoringCoordinator>> GetTutoringCoordinatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null);
        Task<TutoringCoordinator> Get(string tutoringCoordinatorId);
        Task<TutoringCoordinator> GetByCareerId(Guid careerId);
        Task<TutoringCoordinator> GetWithUserById(string tutoringCoordinatorId);
        Task Insert(TutoringCoordinator tutoringCoordinator);
        Task Update(TutoringCoordinator tutoringCoordinator);
        Task DeleteById(string tutoringCoordinatorId);
        Task Delete(TutoringCoordinator tutoringCoordinator);
        Task<TutoringCoordinator> Add(TutoringCoordinator tutoringCoordinator);
        Task<bool> Any(string tutorId);
        Task<bool> AnyByCareerId(Guid careerId);
        SelectList GetTypeTime();
        Task<TutoringCoordinator> GetWithData(string id);
        Task<string> GetCareerByUserId(string userId);
    }
}
