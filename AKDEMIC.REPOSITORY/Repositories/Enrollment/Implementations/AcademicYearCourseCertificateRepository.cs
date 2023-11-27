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
    public class AcademicYearCourseCertificateRepository : Repository<AcademicYearCourseCertificate>, IAcademicYearCourseCertificateRepository
    {
        public AcademicYearCourseCertificateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<AcademicYearCourseCertificate>> GetCourseCertificates(Guid academicYearCourseId)
        {
            var result = await _context.AcademicYearCourseCertificates
                .Where(x => x.AcademicYearCourseId == academicYearCourseId)
                .ToListAsync();

            return result;
        }
    }
}
