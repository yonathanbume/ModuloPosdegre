using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationRatingScaleRepository : IRepository<PerformanceEvaluationRatingScale>
    {
        Task<List<PerformanceEvaluationRatingScale>> GetRaitingScaleByMax(byte max);
    }
}
