using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleExtraInstitutionExperienceFieldRepository : Repository<ScaleExtraInstitutionExperienceField>, IScaleExtraInstitutionExperienceFieldRepository
    {
        public ScaleExtraInstitutionExperienceFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<ScaleExtraInstitutionExperienceField> GetScaleExtraInstitutionExperienceFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraInstitutionExperienceFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }
    }
}
