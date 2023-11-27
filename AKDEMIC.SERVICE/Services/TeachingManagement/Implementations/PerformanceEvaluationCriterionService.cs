using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationCriterionService : IPerformanceEvaluationCriterionService
    {
        private readonly IPerformanceEvaluationCriterionRepository _repository;

        public PerformanceEvaluationCriterionService(IPerformanceEvaluationCriterionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AnyByName(string name, Guid templateId ,Guid? ignoredId)
            => await _repository.AnyByName(name, templateId, ignoredId);

        public async Task<bool> AnyQuestions(Guid id)
            => await _repository.AnyQuestions(id);

        public async Task Delete(PerformanceEvaluationCriterion entity)
            => await _repository.Delete(entity);

        public async Task<PerformanceEvaluationCriterion> Get(Guid id)
            => await _repository.Get(id);

        public async Task<List<PerformanceEvaluationCriterion>> GetAll(Guid templateId)
            => await _repository.GetAll(templateId);

        public async Task<List<PerformanceEvaluationCriterion>> GetCriterions(Guid templateId)
            => await _repository.GetCriterions(templateId);

        public async Task<Select2Structs.ResponseParameters> GetCriterionsSelect2(Select2Structs.RequestParameters parameters, Guid templateId, string search)
            => await _repository.GetCriterionsSelect2(parameters, templateId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid templateId, string searchValue)
            => await _repository.GetDatatable(parameters, templateId, searchValue);

        public async Task Insert(PerformanceEvaluationCriterion entity)
            => await _repository.Insert(entity);

        public async Task Update(PerformanceEvaluationCriterion entity)
            => await _repository.Update(entity);
    }
}
