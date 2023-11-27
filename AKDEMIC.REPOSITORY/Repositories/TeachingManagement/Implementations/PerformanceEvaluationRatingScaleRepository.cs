using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class PerformanceEvaluationRatingScaleRepository : Repository<PerformanceEvaluationRatingScale> ,IPerformanceEvaluationRatingScaleRepository
    {
        public PerformanceEvaluationRatingScaleRepository(AkdemicContext context) : base(context) { }

        public async Task<List<PerformanceEvaluationRatingScale>> GetRaitingScaleByMax(byte max)
        {
            var scales = await _context.PerformanceEvaluationRatingScales.Where(x => x.MaxScore == max)
                .OrderByDescending(x=>x.Value)
                .ToListAsync();

            if(scales.Any() && scales.Count() == max)
            {
                return scales;
            }

            return null;
        }
    }
}
