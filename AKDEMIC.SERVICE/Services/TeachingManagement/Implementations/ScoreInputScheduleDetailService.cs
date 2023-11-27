using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class ScoreInputScheduleDetailService : IScoreInputScheduleDetailService
    {
        private readonly IScoreInputScheduleDetailRepository _scoreInputScheduleDetailRepository;
        public ScoreInputScheduleDetailService(IScoreInputScheduleDetailRepository scoreInputScheduleDetailRepository)
        {
            _scoreInputScheduleDetailRepository = scoreInputScheduleDetailRepository;
        }

        Task IScoreInputScheduleDetailService.DeleteRange(IEnumerable<ScoreInputScheduleDetail> scoreInputScheduleDetails)
            => _scoreInputScheduleDetailRepository.DeleteRange(scoreInputScheduleDetails);

        Task<IEnumerable<ScoreInputScheduleDetail>> IScoreInputScheduleDetailService.GetAllByFilter(Guid? scoreInputScheduleId)
            => _scoreInputScheduleDetailRepository.GetAllByFilter(scoreInputScheduleId);

        Task IScoreInputScheduleDetailService.UpdateRange(IEnumerable<ScoreInputScheduleDetail> scoreInputScheduleDetails)
            => _scoreInputScheduleDetailRepository.UpdateRange(scoreInputScheduleDetails);
    }
}