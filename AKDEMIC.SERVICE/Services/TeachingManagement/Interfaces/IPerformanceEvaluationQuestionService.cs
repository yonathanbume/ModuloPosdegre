using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationQuestionService
    {
        Task<PerformanceEvaluationQuestion> Get(Guid id);
        Task<object> GetPerformanceEvaluationQuestion(Guid id);
        Task<bool> AnyPerformanceEvaluationQuestionByDescription(string description, Guid templateId ,Guid? id);
        Task<ReturnedData<object>> GetPerformanceEvaluationQuestionsDatatable(SentParameters sentParameters, Guid templateId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(PerformanceEvaluationQuestion entity);
        Task Update(PerformanceEvaluationQuestion entity);
    }
}
