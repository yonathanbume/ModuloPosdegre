using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class ScoreInputScheduleDetailRepository : Repository<ScoreInputScheduleDetail>, IScoreInputScheduleDetailRepository
    {
        public ScoreInputScheduleDetailRepository(AkdemicContext context) : base(context) { }

        async Task<IEnumerable<ScoreInputScheduleDetail>> IScoreInputScheduleDetailRepository.GetAllByFilter(Guid? scoreInputScheduleId)
        {
            var query = _context.ScoreInputScheduleDetails.AsQueryable();

            if (scoreInputScheduleId.HasValue)
                query = query.Where(x => x.ScoreInputScheduleId == scoreInputScheduleId.Value);

            return await query.ToListAsync();
        }
    }
}