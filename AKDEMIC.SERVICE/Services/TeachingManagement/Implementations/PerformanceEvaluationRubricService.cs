using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationRubricService : IPerformanceEvaluationRubricService
    {
        private readonly IPerformanceEvaluationRubricRepository _repository;

        public PerformanceEvaluationRubricService(IPerformanceEvaluationRubricRepository repository)
        {
            _repository = repository;
        }

        public Task<List<PerformanceEvaluationRubric>> GetPerformanceEvaluationRubricsByEvaluation(Guid evaluationId)
            => _repository.GetPerformanceEvaluationRubricsByEvaluation(evaluationId);

        public Task Delete(Guid id)
            => _repository.DeleteById(id);

        public Task DeleteById(Guid id)
            => _repository.DeleteById(id);

        public Task<PerformanceEvaluationRubric> Get(Guid id)
            => _repository.Get(id);

        public Task Insert(PerformanceEvaluationRubric entry)
            => _repository.Insert(entry);

        public Task Update(PerformanceEvaluationRubric entry)
            => _repository.Update(entry);

        public async Task<int> GetMaxRatingScale(Guid evaluationId)
            => await _repository.GetMaxRatingScale(evaluationId);

        public async Task<bool> AnyByName(Guid evaluationId, string name, Guid? ignoredId = null)
            => await _repository.AnyByName(evaluationId, name, ignoredId);
    }
}