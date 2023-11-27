using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringPlanRepository : IRepository<TutoringPlan>
    {
        Task<TutoringPlan> GetByCareerId(Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<TutoringPlan> GetWithData(Guid id);
        Task<bool> AnyByCareer(Guid careerId);
        Task<bool> AnyByCoordinator(string tutorCoordinatorId);
        Task<TutoringPlan> GetByCoordinator(string tutorCoordinatorId);
        Task SaveChangesAsync();
    }
}
