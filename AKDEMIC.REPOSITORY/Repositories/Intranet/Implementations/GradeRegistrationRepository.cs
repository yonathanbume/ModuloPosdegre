using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeRegistrationRepository : Repository<GradeRegistration>, IGradeRegistrationRepository
    {
        public GradeRegistrationRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GradeRegistration>> GetAllByFilter(Guid studentSectionId)
        {
            var studentSection = await _context.StudentSections.FindAsync(studentSectionId);

            var result = await _context.GradeRegistrations
                .Where(x => x.SectionId == studentSection.SectionId)
                .ToListAsync();

            return result;          
        }

        public async Task<GradeRegistration> GetByFilters(Guid sectionId, Guid? evaluationId, string userId)
        {
            var query = _context.GradeRegistrations
                .Where(x => x.SectionId == sectionId);

            if (!string.IsNullOrEmpty(userId)) query = query.Where(x => x.TeacherId == userId);

            if (evaluationId.HasValue) query = query.Where(x => x.EvaluationId == evaluationId);

            var gradeRegistration = await query.FirstOrDefaultAsync();

            return gradeRegistration;
        }
    }
}
