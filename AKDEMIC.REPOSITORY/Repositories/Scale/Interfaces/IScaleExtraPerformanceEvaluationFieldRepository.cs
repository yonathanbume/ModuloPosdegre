using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleExtraPerformanceEvaluationFieldRepository : IRepository<ScaleExtraPerformanceEvaluationField>
    {
        Task<ScaleExtraPerformanceEvaluationField> GetScaleExtraPerformanceEvaluationFieldByResolutionId(Guid resolutionId);

        Task<TeachingPerformanceResultTemplate> GetPerformanceEvaluationResult(Guid? academicDeparmentId, List<Guid> listTerms, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<bool> AnyByCodeAndTerm(string code, Guid termId);

        Task<DataTablesStructs.ReturnedData<object>> GetPerformanceEvaluationPublishedResultsDatatable(DataTablesStructs.SentParameters parameters, Guid performanceEvaluationId, string search);

        Task<List<PerformanceEvaluationResultTemplate>> GetPerformanceEvaluationPublishedResults(Guid performanceEvaluationId);
    }
}
