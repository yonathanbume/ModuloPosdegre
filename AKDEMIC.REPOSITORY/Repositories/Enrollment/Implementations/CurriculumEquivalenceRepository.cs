using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CurriculumEquivalenceRepository : Repository<CurriculumEquivalence>, ICurriculumEquivalenceRepository
    {
        public CurriculumEquivalenceRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<CurriculumEquivalence> GetByNewCurriculumId(Guid curriculumId)
        {
            return await _context.CurriculumEquivalences.FirstOrDefaultAsync(x => x.NewCurriculumId == curriculumId);
        }

        public async Task Insert(CurriculumEquivalence curriculumEquivalence)
        {
            var courseEquivalences = await _context.CourseEquivalences
                .Where(x => x.NewAcademicYearCourse.CurriculumId == curriculumEquivalence.NewCurriculumId)
                .ToListAsync();

            var curriculumEquivalences = await _context.CurriculumEquivalences
                .Where(x => x.NewCurriculumId == curriculumEquivalence.NewCurriculumId)
                .ToListAsync();

            _context.CourseEquivalences.RemoveRange(courseEquivalences);
            _context.CurriculumEquivalences.RemoveRange(curriculumEquivalences);
            await _context.SaveChangesAsync();

            await _context.CurriculumEquivalences.AddAsync(curriculumEquivalence);
            await _context.SaveChangesAsync();
        }
    }
}
