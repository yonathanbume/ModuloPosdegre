using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAcademicYearCoursePreRequisiteService
    {
        Task<AcademicYearCoursePreRequisite> GetAsync(Guid id);
        Task InsertAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite);
        Task InsertRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites);
        Task UpdateAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite);
        Task UpdateRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites);
        Task DeleteAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite);
        Task DeleteRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites);
        Task<IEnumerable<AcademicYearCoursePreRequisite>> GetAllByFilter(byte? academicYear = null, Guid? academicYearCourseId = null, Guid? curriculumId = null);
        Task<bool> AnyByAcademicYearCourseAndCourseId(Guid academicYearCourseId, Guid courseId);
    }
}