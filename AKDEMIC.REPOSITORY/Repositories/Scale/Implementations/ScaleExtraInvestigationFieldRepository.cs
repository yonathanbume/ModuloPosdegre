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
    public class ScaleExtraInvestigationFieldRepository : Repository<ScaleExtraInvestigationField>, IScaleExtraInvestigationFieldRepository
    {
        public ScaleExtraInvestigationFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ScaleExtraInvestigationField>> GetAllByUserId(string userId)
        {
            return await _context.ScaleExtraInvestigationFields
                .Include(x => x.ScaleResolution)
                    .ThenInclude(x => x.ScaleSectionResolutionType)
                        .ThenInclude(x => x.ScaleResolutionType)
                .Include(x => x.InvestigationParticipationType)
                .Where(x => x.ScaleResolution.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScaleExtraInvestigationField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _context.ScaleExtraInvestigationFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == resolutionTypeName
                       && x.ScaleResolution.UserId == userId)
                .ToListAsync();
        }

        public async Task<ScaleExtraInvestigationField> GetScaleExtraInvestigationFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraInvestigationFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }
    }
}
