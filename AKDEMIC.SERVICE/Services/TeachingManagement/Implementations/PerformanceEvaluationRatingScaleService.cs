using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationRatingScaleService : IPerformanceEvaluationRatingScaleService
    {
        private readonly IPerformanceEvaluationRatingScaleRepository _repository;

        public PerformanceEvaluationRatingScaleService(IPerformanceEvaluationRatingScaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PerformanceEvaluationRatingScale>> GetRaitingScaleByMax(byte max)
            => await _repository.GetRaitingScaleByMax(max);
    }
}
