using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleExtraPerformanceEvaluationFieldService
    {
        Task<ScaleExtraPerformanceEvaluationField> Get(Guid id);
        Task Insert(ScaleExtraPerformanceEvaluationField entity);
        Task Update(ScaleExtraPerformanceEvaluationField entity);
        Task Delete(ScaleExtraPerformanceEvaluationField entity);
        Task<ScaleExtraPerformanceEvaluationField> GetScaleExtraPerformanceEvaluationFieldByResolutionId(Guid resolutionId);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<bool> AnyByCodeAndTerm(string code, Guid termId);

        Task<TeachingPerformanceResultTemplate> GetPerformanceEvaluationResult(Guid? academicDeparmentId, List<Guid> listTerms, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetPerformanceEvaluationPublishedResultsDatatable(DataTablesStructs.SentParameters parameters, Guid performanceEvaluationId, string search);

        Task<List<PerformanceEvaluationResultTemplate>> GetPerformanceEvaluationPublishedResults(Guid performanceEvaluationId);
    }
}
