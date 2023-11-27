using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class MeetingCriterionRepository : Repository<MeetingCriterion>, IMeetingCriterionRepository
    {
        public MeetingCriterionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByMeetingId(Guid meetingId)
            => await _context.MeetingCriterions.Where(x => x.MeetingId == meetingId).ToArrayAsync();

        public async Task<IEnumerable<MeetingCriterion>> GetMeetingCriterionsByCriterionId(Guid criterionId)
            => await _context.MeetingCriterions.Where(x => x.CriterionId == criterionId).ToArrayAsync();
    }
}
