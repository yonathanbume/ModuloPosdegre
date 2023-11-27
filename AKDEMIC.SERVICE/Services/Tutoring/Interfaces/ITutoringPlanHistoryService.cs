using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringPlanHistoryService
    {
        Task<IEnumerable<TutoringPlanHistory>> GetAll();
        Task<TutoringPlanHistory> Get(Guid id);
        Task<List<TutoringPlanHistory>> GetHistoriesByTutoringPlan(Guid tutoringPlanId);
        Task Insert(TutoringPlanHistory tutoringPlanHistory);
        Task Update(TutoringPlanHistory tutoringPlanHistory);
        Task Delete(TutoringPlanHistory tutoringPlanHistory);
        Task DeleteRange(List<TutoringPlanHistory> tutoringPlanHistories);
        Task<TutoringPlanHistory> AddAsync(TutoringPlanHistory tutoringPlanHistory);
        Task SaveChangesAsync();
    }
}
