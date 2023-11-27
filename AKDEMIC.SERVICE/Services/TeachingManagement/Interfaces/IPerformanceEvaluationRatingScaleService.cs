using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationRatingScaleService
    {
        Task<List<PerformanceEvaluationRatingScale>> GetRaitingScaleByMax(byte max);
    }
}
