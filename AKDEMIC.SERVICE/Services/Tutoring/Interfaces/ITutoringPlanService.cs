using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringPlanService
    {
        Task<IEnumerable<TutoringPlan>> GetAll();
        Task<TutoringPlan> Get(Guid tutoringPlanId);
        Task<TutoringPlan> GetByCareerId(Guid careerId);
        Task Insert(TutoringPlan tutoringPlan);
        Task Update(TutoringPlan tutoringPlan);
        Task DeleteById(Guid tutoringPlanId);
        Task Delete(TutoringPlan tutoringPlan);
        Task<TutoringPlan> AddAsync(TutoringPlan tutoringPlan);
        Task SaveChangesAsync();
        Task<TutoringPlan> GetFirst();
        Task<DataTablesStructs.ReturnedData<object>> GetPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<TutoringPlan> GetWithData(Guid id);
        Task<bool> AnyByCareer(Guid careerId);
        Task<bool> AnyByCoordinator(string tutorCoordinatorId);
        Task<TutoringPlan> GetByCoordinator(string tutorCoordinatorId);
    }
}
