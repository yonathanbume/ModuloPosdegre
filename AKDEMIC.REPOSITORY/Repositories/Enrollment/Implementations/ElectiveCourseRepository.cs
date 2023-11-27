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
    public class ElectiveCourseRepository : Repository<ElectiveCourse>, IElectiveCourseRepository
    {
        public ElectiveCourseRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ElectiveCourse>> GetAllElectiveCoursesWithData()
        {
            var result = await _context.ElectiveCourses
                .Include(x => x.Course).ToListAsync();

            return result;
        }

        public async Task<List<ElectiveCourse>> GetWithDataByCareerIdAndAcadmic(Guid careerId, int academicYear, int academicYearDispersion)
        {
            var electiveCourses = await _context.ElectiveCourses
                .Include(x => x.Course)
                .Where(ec => ec.CareerId == careerId && ec.Active && ec.AcademicYearNumber <= academicYear + academicYearDispersion).ToListAsync();

            return electiveCourses;
        }
    }
}
