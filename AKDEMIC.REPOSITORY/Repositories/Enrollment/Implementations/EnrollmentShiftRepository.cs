using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentShiftRepository : Repository<EnrollmentShift>, IEnrollmentShiftRepository
    {
        public EnrollmentShiftRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTerm(Guid termId)
        {
            return await _context.EnrollmentShifts.AnyAsync(e => e.TermId == termId);
        }

        public async Task<EnrollmentShift> GetByTerm(Guid termId)
        {
            return await _context.EnrollmentShifts.Include(x => x.Term).FirstOrDefaultAsync(e => e.TermId == termId);
        }

        public async Task<EnrollmentShift> GetActiveTermEnrollmentShift()
        {
            var enrollmentShift = await _context.EnrollmentShifts
                .FirstOrDefaultAsync(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (enrollmentShift == null)
            {
                var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

                enrollmentShift = new EnrollmentShift
                {
                    TermId = term.Id
                };

                await _context.EnrollmentShifts.AddAsync(enrollmentShift);
                await _context.SaveChangesAsync();
            }

            return enrollmentShift;
        }
    }
}
