using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseUnitService
    {
        Task<bool> AnyByCourseSyllabusId(Guid courseSyllabusId);
        Task<object> GetCourseUnitsSelect2ClientSide(Guid courseId, Guid termId);
        Task<object> GetCourseUnitsSelect2ClientSide2(Guid courseId, Guid termId);
        Task<IEnumerable<CourseUnit>> GetAllBySyllabusId(Guid syllabusId);
        Task<IEnumerable<CourseUnit>> GetAllBySyllabusId2(Guid syllabusId);
        Task<IEnumerable<CourseUnit>> GetCourseUnitProgressBySectionIdAndSyllabusId(Guid syllabusId, Guid sectionId);
        Task<CourseUnit> GetAsync(Guid id);
        Task<IEnumerable<CourseUnitModelA>> GetAllAsModelA(Guid? courseId = null, Guid? termId = null);
        Task InsertAsync(CourseUnit courseUnit);
        Task InsertRangeAsync(IEnumerable<CourseUnit> courseUnits);
        Task UpdateAsync(CourseUnit courseUnit);
        Task UpdateRangeAsync(IEnumerable<CourseUnit> courseUnits);
        Task DeleteAsync(CourseUnit courseUnit);
        Task DeleteRangeAsync(IEnumerable<CourseUnit> courseUnits);
        Task<CourseUnit> GetInDateRangeBySyllabus(int weekNumberStart, int weekNumberEnd, Guid syllabusId, Guid? id = null);
        Task<object> GetAsModelB(Guid id);
        Task<IEnumerable<CourseUnitGrades>> GetCourseUnitGradesByStudentIdAndSectionId(Guid studentId, Guid sectionId);
        Task<int> GetQuantityCourseUnits(Guid courseId, Guid termId);
        Task DeleteUnitWithData(Guid unitId);
        Task<List<CourseUnit>> GetCourseUnits(Guid courseId, Guid termId);
        Task<int> GetTotalAcademicProgressPercentage(Guid courseSyllabusId, Guid? ignoredId = null);

    }
}