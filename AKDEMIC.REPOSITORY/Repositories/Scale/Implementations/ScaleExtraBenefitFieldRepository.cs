using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleExtraBenefitFieldRepository : Repository<ScaleExtraBenefitField>, IScaleExtraBenefitFieldRepository
    {
        public ScaleExtraBenefitFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ScaleExtraBenefitField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _context.ScaleExtraBenefitFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == resolutionTypeName
                        && x.ScaleResolution.UserId == userId)
                .OrderBy(x => x.ScaleResolution.ExpeditionDate)
                .ToListAsync();
        }

        public async Task<ScaleExtraBenefitField> GetScaleExtraBenefitFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraBenefitFields
                .Include(x => x.BenefitType)
                .FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }
    }
}
