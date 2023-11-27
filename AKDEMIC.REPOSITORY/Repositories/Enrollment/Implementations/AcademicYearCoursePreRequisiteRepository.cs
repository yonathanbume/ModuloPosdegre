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
    public sealed class AcademicYearCoursePreRequisiteRepository : Repository<AcademicYearCoursePreRequisite>, IAcademicYearCoursePreRequisiteRepository
    {
        public AcademicYearCoursePreRequisiteRepository(AkdemicContext context) : base(context) { }

        Task<bool> IAcademicYearCoursePreRequisiteRepository.AnyByAcademicYearCourseAndCourseId(Guid academicYearCourseId, Guid courseId)
            => _context.AcademicYearCoursePreRequisites.AnyAsync(x => x.AcademicYearCourseId == academicYearCourseId && x.CourseId == courseId);

        async Task<IEnumerable<AcademicYearCoursePreRequisite>> IAcademicYearCoursePreRequisiteRepository.GetAllByFilter(byte? academicYear, Guid? academicYearCourseId, Guid? curriculumId)
        {
            var query = _context.AcademicYearCoursePreRequisites.Include(x=>x.Course).AsQueryable();

            if (academicYear.HasValue)
                query = query.Where(x => x.AcademicYearCourse.AcademicYear == academicYear.Value);

            if (academicYearCourseId.HasValue)
                query = query.Where(x => x.AcademicYearCourseId == academicYearCourseId.Value);

            if (curriculumId.HasValue)
                query = query.Where(x => x.AcademicYearCourse.CurriculumId == curriculumId.Value);

            return await query.ToListAsync();
        }
    }
}