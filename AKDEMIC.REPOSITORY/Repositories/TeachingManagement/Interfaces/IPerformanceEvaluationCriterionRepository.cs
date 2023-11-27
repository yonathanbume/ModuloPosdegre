using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationCriterionRepository : IRepository<PerformanceEvaluationCriterion>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid templateId, string searchValue);
        Task<bool> AnyByName(string name, Guid templateId, Guid? ignoredId);
        Task<bool> AnyQuestions(Guid id);
        Task<List<PerformanceEvaluationCriterion>> GetCriterions(Guid templateId);
        Task<Select2Structs.ResponseParameters> GetCriterionsSelect2(Select2Structs.RequestParameters parameters, Guid templateId, string search);
        Task<List<PerformanceEvaluationCriterion>> GetAll(Guid templateId);
    }
}
