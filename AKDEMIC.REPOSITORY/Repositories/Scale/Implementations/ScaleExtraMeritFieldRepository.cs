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
    public class ScaleExtraMeritFieldRepository : Repository<ScaleExtraMeritField>, IScaleExtraMeritFieldRepository
    {
        public ScaleExtraMeritFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<ScaleExtraMeritField> GetScaleExtraMeritFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraMeritFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraMeritField>> GetByUserId(string userId)
        {
            return await _context.ScaleExtraMeritFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.ScaleResolution.UserId == userId)
                .ToListAsync();
        }
    }
}
