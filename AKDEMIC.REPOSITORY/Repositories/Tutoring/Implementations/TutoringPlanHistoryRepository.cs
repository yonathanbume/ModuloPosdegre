using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringPlanHistoryRepository: Repository<TutoringPlanHistory>, ITutoringPlanHistoryRepository
    {
        public TutoringPlanHistoryRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<TutoringPlanHistory>> GetHistoriesByTutoringPlan(Guid tutoringPlanId)
        {
            var result = await _context.TutoringPlanHistories
                .Where(x => x.TutoringPlanId == tutoringPlanId)
                .ToListAsync();

            return result;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
