using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationRubricRepository
    {
        Task<PerformanceEvaluationRubric> Get(Guid id);
        Task<List<PerformanceEvaluationRubric>> GetPerformanceEvaluationRubricsByEvaluation(Guid evaluationId);
        Task DeleteById(Guid id);
        Task Insert(PerformanceEvaluationRubric entity);
        Task Update(PerformanceEvaluationRubric entity);
        Task<int> GetMaxRatingScale(Guid evaluationId);
        Task<bool> AnyByName(Guid evaluationId, string name, Guid? ignoredId = null);
    }
}
