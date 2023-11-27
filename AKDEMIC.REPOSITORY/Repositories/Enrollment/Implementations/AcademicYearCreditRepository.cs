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
    public class AcademicYearCreditRepository : Repository<AcademicYearCredit>, IAcademicYearCreditRepository
    {
        public AcademicYearCreditRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<AcademicYearCredit>> GetCurriculumAcademicYearCredits(Guid curriculumId)
        {
            var result = await _context.AcademicYearCredits
                .Where(x => x.CurriculumId == curriculumId)
                .ToListAsync();
            return result;
        }
    }
}
