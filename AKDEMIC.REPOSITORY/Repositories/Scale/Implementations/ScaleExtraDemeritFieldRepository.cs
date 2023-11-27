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
    public class ScaleExtraDemeritFieldRepository : Repository<ScaleExtraDemeritField>, IScaleExtraDemeritFieldRepository
    {
        public ScaleExtraDemeritFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<ScaleExtraDemeritField> GetScaleExtraDemeritFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraDemeritFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraDemeritField>> GetByUserId(string userId)
        {
            return  await _context.ScaleExtraDemeritFields
                .Include(x => x.ScaleResolution)
                .Where(x => x.ScaleResolution.UserId == userId)
                .ToListAsync();
        }
    }
}
