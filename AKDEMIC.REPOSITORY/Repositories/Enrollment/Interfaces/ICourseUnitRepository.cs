using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseUnitRepository : IRepository<CourseUnit>
    {
        Task<bool> AnyByCourseSyllabusId(Guid courseSyllabusId);
        Task<object> GetCourseUnitsSelect2ClientSide(Guid courseId, Guid termId);
        Task<object> GetCourseUnitsSelect2ClientSide2(Guid courseId, Guid termId);
        Task<IEnumerable<CourseUnit>> GetAllBySyllabusId(Guid syllabusId);
        Task<IEnumerable<CourseUnit>> GetAllBySyllabusId2(Guid syllabusId);
        Task<IEnumerable<CourseUnit>> GetCourseUnitProgressBySectionIdAndSyllabusId(Guid syllabusId, Guid sectionId);
        Task<IEnumerable<CourseUnitModelA>> GetAllAsModelA(Guid? courseId = null, Guid? termId = null);
        Task<CourseUnit> GetInDateRangeBySyllabus(int weekNumberStart, int weekNumberEnd, Guid syllabusId, Guid? id = null);
        Task<object> GetAsModelB(Guid id);
        Task<IEnumerable<CourseUnitGrades>> GetCourseUnitGradesByStudentIdAndSectionId(Guid studentId, Guid sectionId);
        Task<int> GetQuantityCourseUnits(Guid courseId, Guid termId);
        Task DeleteUnitWithData(Guid unitId);
        Task<List<CourseUnit>> GetCourseUnits(Guid courseId, Guid termId);
        Task<int> GetTotalAcademicProgressPercentage(Guid courseSyllabusId, Guid? ignoredId = null);
    }
}