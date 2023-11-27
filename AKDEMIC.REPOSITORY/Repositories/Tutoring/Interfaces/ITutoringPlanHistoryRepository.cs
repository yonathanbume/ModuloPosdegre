using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringPlanHistoryRepository:IRepository<TutoringPlanHistory>
    {
        Task<List<TutoringPlanHistory>> GetHistoriesByTutoringPlan(Guid tutoringPlanId);
        Task SaveChangesAsync();
    }
}
