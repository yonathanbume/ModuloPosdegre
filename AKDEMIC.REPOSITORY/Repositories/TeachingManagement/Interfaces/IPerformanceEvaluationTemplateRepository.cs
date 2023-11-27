using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationTemplateRepository
    {
        Task<PerformanceEvaluationTemplate> Get(Guid id);
        Task<object> GetPerformanceEvaluationTemplate(Guid id);
        Task<bool> AnyPerformanceEvaluationTemplateByName(string name, Guid? id);
        Task<ReturnedData<object>> GetPerformanceEvaluationTemplatesDatatable(SentParameters sentParameters, string searchValue = null, Guid? performanceEvaluationId = null);
        Task<PerformanceEvaluationTemplate> GetActiveTemplateByRole(string roleName);
        Task Active(Guid id);
        Task<string> ValidateActiveTemplates(byte target);
        Task DeleteById(Guid id);
        Task Insert(PerformanceEvaluationTemplate entity);
        Task Update(PerformanceEvaluationTemplate entity);
        Task<PerformanceEvaluationTemplate> GetPerformanceEvaluationWithQuestions(Guid templateId);
        Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplateActive(byte target);
        Task<PerformanceEvaluationTemplate> ImportEvaluationPerformanceTemplate(Guid oldTemplateId, string newName);
        Task<Select2Structs.ResponseParameters> GetTemplatesSelet2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<List<PerformanceEvaluationTemplate>> GetPerformanceEvaluationTemplates(Guid evaluationId);
    }
}
