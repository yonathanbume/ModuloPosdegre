using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EntrantEnrollmentRepository : Repository<EntrantEnrollment>, IEntrantEnrollmentRepository
    {

        public EntrantEnrollmentRepository(AkdemicContext context) : base (context) { }

        public async Task<IEnumerable<EntrantEnrollment>> GetAllWithData(Guid careerId, Guid? termId = null)
        {
            var result = await _context.EntrantEnrollments
                .Where(ee => ee.CareerId == careerId && ee.TermId == termId)
                .Include(x => x.Term)
                .Include(x => x.Career)
                .ThenInclude(x => x.Faculty).ToListAsync();

            return result;
        }

        public async Task<EntrantEnrollment> GetEnrollmentByTermidAndCareerId(Guid careerId, Guid? termId = null)
        {
            var result = await _context.EntrantEnrollments
                .FirstOrDefaultAsync(x => x.TermId == termId && x.CareerId == careerId);

            return result;
        }
    }
}
