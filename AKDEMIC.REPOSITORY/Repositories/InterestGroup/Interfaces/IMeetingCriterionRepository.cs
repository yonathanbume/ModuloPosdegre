using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IMeetingCriterionRepository : IRepository<MeetingCriterion>
    {
        Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByMeetingId(Guid meetingId);
        Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByCriterionId(Guid criterionId);
    }
}
