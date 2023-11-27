using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraPerformanceEvaluationFieldService : IScaleExtraPerformanceEvaluationFieldService
    {
        private readonly IScaleExtraPerformanceEvaluationFieldRepository _scaleExtraPerformanceEvaluationFieldRepository;

        public ScaleExtraPerformanceEvaluationFieldService(IScaleExtraPerformanceEvaluationFieldRepository scaleExtraPerformanceEvaluationFieldRepository)
        {
            _scaleExtraPerformanceEvaluationFieldRepository = scaleExtraPerformanceEvaluationFieldRepository;
        }

        public async Task<ScaleExtraPerformanceEvaluationField> Get(Guid id)
        {
            return await _scaleExtraPerformanceEvaluationFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraPerformanceEvaluationField entity)
        {
            await _scaleExtraPerformanceEvaluationFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraPerformanceEvaluationField entity)
        {
            await _scaleExtraPerformanceEvaluationFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraPerformanceEvaluationField entity)
        {
            await _scaleExtraPerformanceEvaluationFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraPerformanceEvaluationField> GetScaleExtraPerformanceEvaluationFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraPerformanceEvaluationFieldRepository.GetScaleExtraPerformanceEvaluationFieldByResolutionId(resolutionId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _scaleExtraPerformanceEvaluationFieldRepository.GetEvaluationRecordByUser(sentParameters,userId,searchValue);
        }

        public async Task<bool> AnyByCodeAndTerm(string code, Guid termId)
            => await _scaleExtraPerformanceEvaluationFieldRepository.AnyByCodeAndTerm(code, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPerformanceEvaluationPublishedResultsDatatable(DataTablesStructs.SentParameters parameters, Guid performanceEvaluationId, string search)
            => await _scaleExtraPerformanceEvaluationFieldRepository.GetPerformanceEvaluationPublishedResultsDatatable(parameters, performanceEvaluationId, search);

        public async Task<List<PerformanceEvaluationResultTemplate>> GetPerformanceEvaluationPublishedResults(Guid performanceEvaluationId)
            => await _scaleExtraPerformanceEvaluationFieldRepository.GetPerformanceEvaluationPublishedResults(performanceEvaluationId);

        public async Task<TeachingPerformanceResultTemplate> GetPerformanceEvaluationResult(Guid? academicDeparmentId, List<Guid> listTerms, string search)
            => await _scaleExtraPerformanceEvaluationFieldRepository.GetPerformanceEvaluationResult(academicDeparmentId, listTerms, search);
    }
}
