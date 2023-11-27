using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtraordinaryEvaluationCommitteeRepository : Repository<ExtraordinaryEvaluationCommittee>, IExtraordinaryEvaluationCommitteeRepository
    {
        public ExtraordinaryEvaluationCommitteeRepository(AkdemicContext context) : base(context) {  }

        public async Task<IEnumerable<ExtraordinaryEvaluationCommittee>> GetCommittee(Guid extraordinaryEvalutionId)
            => await _context.ExtraordinaryEvaluationCommittees.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvalutionId).Include(x=>x.Teacher.User).ToListAsync();

        public async Task DeleteCommitteeByEvalutionId(Guid extraordinaryEvalutionId)
        {
            var commite = await _context.ExtraordinaryEvaluationCommittees.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvalutionId).ToListAsync();
            if (commite.Any())
            {
                _context.ExtraordinaryEvaluationCommittees.RemoveRange(commite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
