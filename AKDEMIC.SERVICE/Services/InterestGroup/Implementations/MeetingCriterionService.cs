using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class MeetingCriterionService : IMeetingCriterionService
    {
        private readonly IMeetingCriterionRepository _meetingCriterionRepository;

        public MeetingCriterionService(IMeetingCriterionRepository meetingCriterionRepository)
        {
            _meetingCriterionRepository = meetingCriterionRepository;
        }

        public async Task DeleteRange(IEnumerable<MeetingCriterion> meetingCriterions)
            => await _meetingCriterionRepository.DeleteRange(meetingCriterions);

        public async Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByCriterionId(Guid criterionId)
            => await _meetingCriterionRepository.GetMeetingCriterionsByCriterionId(criterionId);

        public async Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByMeetingId(Guid meetingId)
            => await _meetingCriterionRepository.GetMeetingCriterionsByMeetingId(meetingId);
    }
}
