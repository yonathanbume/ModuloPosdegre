using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleSectionResolutionTypeRepository : Repository<ScaleSectionResolutionType>, IScaleSectionResolutionTypeRepository
    {
        public ScaleSectionResolutionTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> IsActive(Guid sectionId, Guid resolutionTypeId)
        {
            return await _context.ScaleSectionResolutionTypes.AnyAsync(x => x.ScaleSectionId == sectionId && x.ScaleResolutionTypeId == resolutionTypeId && x.Status == ConstantHelpers.STATES.ACTIVE);
        }

        public async Task<ScaleSectionResolutionType> GetByRelations(Guid sectionId, Guid resolutionTypeId)
        {
            return await _context.ScaleSectionResolutionTypes
                .Include(x => x.ScaleResolutionType)
                .FirstOrDefaultAsync(x => x.ScaleSectionId == sectionId && x.ScaleResolutionTypeId == resolutionTypeId);
        }

        public async Task DeleteByRelations(Guid sectionId, Guid resolutionTypeId)
        {
            var entity = await _context.ScaleSectionResolutionTypes.FirstOrDefaultAsync(x => x.ScaleResolutionTypeId == resolutionTypeId && x.ScaleSectionId == sectionId);
            _context.ScaleSectionResolutionTypes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ScaleSectionResolutionType> GetIncludeResolutionType(Guid id)
            => await _context.ScaleSectionResolutionTypes.Include(x => x.ScaleResolutionType).Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}
