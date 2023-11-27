using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IMeetingCriterionService
    {
        Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByMeetingId(Guid meetingId);
        Task DeleteRange(IEnumerable<MeetingCriterion> meetingCriterions);
        Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByCriterionId(Guid criterionId);
    }
}
