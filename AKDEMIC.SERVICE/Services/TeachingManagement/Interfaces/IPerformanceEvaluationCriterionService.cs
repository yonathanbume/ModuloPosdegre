using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationCriterionService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid templateId, string searchValue);
        Task<bool> AnyByName(string name, Guid templateId, Guid? ignoredId);
        Task<bool> AnyQuestions(Guid id);
        Task Insert(PerformanceEvaluationCriterion entity);
        Task Update(PerformanceEvaluationCriterion entity);
        Task<PerformanceEvaluationCriterion> Get(Guid id);
        Task Delete(PerformanceEvaluationCriterion entity);
        Task<List<PerformanceEvaluationCriterion>> GetCriterions(Guid templateId);
        Task<Select2Structs.ResponseParameters> GetCriterionsSelect2(Select2Structs.RequestParameters parameters, Guid templateId, string search);
        Task<List<PerformanceEvaluationCriterion>> GetAll(Guid templateId);
    }
}
