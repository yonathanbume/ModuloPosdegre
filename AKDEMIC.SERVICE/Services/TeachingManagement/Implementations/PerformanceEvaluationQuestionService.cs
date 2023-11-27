using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationQuestionService : IPerformanceEvaluationQuestionService
    {
        private readonly IPerformanceEvaluationQuestionRepository _repository;

        public PerformanceEvaluationQuestionService(IPerformanceEvaluationQuestionRepository repository)
        {
            _repository = repository;
        }
        public async Task<PerformanceEvaluationQuestion> Get(Guid id)
        {
            return await _repository.Get(id);
        }
        public async Task<object> GetPerformanceEvaluationQuestion(Guid id)
        {
            return await _repository.GetPerformanceEvaluationQuestion(id);
        }
        public async Task<bool> AnyPerformanceEvaluationQuestionByDescription(string description, Guid templateId ,Guid? id)
        {
            return await _repository.AnyPerformanceEvaluationQuestionByDescription(description, templateId,id);
        }
        public async Task<ReturnedData<object>> GetPerformanceEvaluationQuestionsDatatable(SentParameters sentParameters, Guid templateId, string searchValue = null)
        {
            return await _repository.GetPerformanceEvaluationQuestionsDatatable(sentParameters, templateId, searchValue);
        }
        public async Task DeleteById(Guid id)
        {
            await _repository.DeleteById(id);
        }
        public async Task Insert(PerformanceEvaluationQuestion entity)
        {
            await _repository.Insert(entity);
        }
        public async Task Update(PerformanceEvaluationQuestion entity)
        {
            await _repository.Update(entity);
        }
    }
}
