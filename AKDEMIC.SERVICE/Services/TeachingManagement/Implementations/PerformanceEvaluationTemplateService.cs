using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationTemplateService : IPerformanceEvaluationTemplateService
    {
        private readonly IPerformanceEvaluationTemplateRepository _repository;

        public PerformanceEvaluationTemplateService(IPerformanceEvaluationTemplateRepository repository)
        {
            _repository = repository;
        }
        public async Task<PerformanceEvaluationTemplate> Get(Guid id)
        {
            return await _repository.Get(id);
        }
        public async Task<object> GetPerformanceEvaluationTemplate(Guid id)
        {
            return await _repository.GetPerformanceEvaluationTemplate(id);
        }
        public async Task<bool> AnyPerformanceEvaluationTemplateByName(string name, Guid? id)
        {
            return await _repository.AnyPerformanceEvaluationTemplateByName(name, id);
        }
        public async Task<ReturnedData<object>> GetPerformanceEvaluationTemplatesDatatable(SentParameters sentParameters, string searchValue = null, Guid? performanceEvaluationId = null)
        {
            return await _repository.GetPerformanceEvaluationTemplatesDatatable(sentParameters, searchValue, performanceEvaluationId);
        }
        public async Task<PerformanceEvaluationTemplate> GetActiveTemplateByRole(string roleName)
        {
            return await _repository.GetActiveTemplateByRole(roleName);
        }
        public async Task Active(Guid id)
        {
            await _repository.Active(id);
        }
        public async Task<string> ValidateActiveTemplates(byte target)
        {
            return await _repository.ValidateActiveTemplates(target);
        }
        public async Task DeleteById(Guid id)
        {
            await _repository.DeleteById(id);
        }
        public async Task Insert(PerformanceEvaluationTemplate entity)
        {
            await _repository.Insert(entity);
        }
        public async Task Update(PerformanceEvaluationTemplate entity)
        {
            await _repository.Update(entity);
        }

        public async Task<PerformanceEvaluationTemplate> GetPerformanceEvaluationWithQuestions(Guid templateId)
            => await _repository.GetPerformanceEvaluationWithQuestions(templateId);

        public async Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplateActive(byte target)
            => await _repository.GetPerformanceEvaluationTemplateActive(target);

        public async Task<Select2Structs.ResponseParameters> GetTemplatesSelet2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => await _repository.GetTemplatesSelet2(requestParameters, searchValue);

        public async Task<PerformanceEvaluationTemplate> ImportEvaluationPerformanceTemplate(Guid oldTemplateId, string newName)
            => await _repository.ImportEvaluationPerformanceTemplate(oldTemplateId, newName);

        public async Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplates(Guid evaluationId)
            => await _repository.GetPerformanceEvaluationTemplates(evaluationId);
    }
}
