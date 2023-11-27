using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseUnitService : ICourseUnitService
    {
        private readonly ICourseUnitRepository _courseUnitRepository;

        public CourseUnitService(ICourseUnitRepository courseUnitRepository)
        {
            _courseUnitRepository = courseUnitRepository;
        }

        public Task<bool> AnyByCourseSyllabusId(Guid courseSyllabusId)
            => _courseUnitRepository.AnyByCourseSyllabusId(courseSyllabusId);

        public async Task DeleteAsync(CourseUnit courseUnit)
            => await _courseUnitRepository.Delete(courseUnit);

        public Task DeleteRangeAsync(IEnumerable<CourseUnit> courseUnits)
            => _courseUnitRepository.DeleteRange(courseUnits);

        public async Task DeleteUnitWithData(Guid unitId) => await _courseUnitRepository.DeleteUnitWithData(unitId);

        public async Task<List<CourseUnit>> GetCourseUnits(Guid courseId, Guid termId)
        {
            return await _courseUnitRepository.GetCourseUnits(courseId, termId);
        }

        public async Task<IEnumerable<CourseUnitModelA>> GetAllAsModelA(Guid? courseId = null, Guid? termId = null)
            => await _courseUnitRepository.GetAllAsModelA(courseId, termId);

        public async Task<IEnumerable<CourseUnit>> GetAllBySyllabusId(Guid syllabusId)
            => await _courseUnitRepository.GetAllBySyllabusId(syllabusId);

        public async Task<IEnumerable<CourseUnit>> GetAllBySyllabusId2(Guid syllabusId)
            => await _courseUnitRepository.GetAllBySyllabusId2(syllabusId);

        public Task<object> GetAsModelB(Guid id)
            => _courseUnitRepository.GetAsModelB(id);

        public async Task<CourseUnit> GetAsync(Guid id)
            => await _courseUnitRepository.Get(id);

        public async Task<IEnumerable<CourseUnitGrades>> GetCourseUnitGradesByStudentIdAndSectionId(Guid studentId, Guid sectionId)
            => await _courseUnitRepository.GetCourseUnitGradesByStudentIdAndSectionId(studentId, sectionId);

        public async Task<IEnumerable<CourseUnit>> GetCourseUnitProgressBySectionIdAndSyllabusId(Guid syllabusId, Guid sectionId)
            => await _courseUnitRepository.GetCourseUnitProgressBySectionIdAndSyllabusId(syllabusId, sectionId);

        public async Task<object> GetCourseUnitsSelect2ClientSide(Guid courseId, Guid termId)
            => await _courseUnitRepository.GetCourseUnitsSelect2ClientSide(courseId, termId);

        public async Task<object> GetCourseUnitsSelect2ClientSide2(Guid courseId, Guid termId)
            => await _courseUnitRepository.GetCourseUnitsSelect2ClientSide2(courseId, termId);

        public Task<CourseUnit> GetInDateRangeBySyllabus(int weekNumberStart, int weekNumberEnd, Guid syllabusId, Guid? id = null)
            => _courseUnitRepository.GetInDateRangeBySyllabus(weekNumberStart, weekNumberEnd, syllabusId, id);

        public async Task<int> GetQuantityCourseUnits(Guid courseId, Guid termId)
            => await _courseUnitRepository.GetQuantityCourseUnits(courseId, termId);

        public Task InsertAsync(CourseUnit courseUnit)
            => _courseUnitRepository.Insert(courseUnit);

        public Task InsertRangeAsync(IEnumerable<CourseUnit> courseUnits)
            => _courseUnitRepository.InsertRange(courseUnits);

        public Task UpdateAsync(CourseUnit courseUnit)
            => _courseUnitRepository.Update(courseUnit);

        public Task UpdateRangeAsync(IEnumerable<CourseUnit> courseUnits)
            => _courseUnitRepository.UpdateRange(courseUnits);

        public async Task<int> GetTotalAcademicProgressPercentage(Guid courseSyllabusId, Guid? ignoredId = null)
            => await _courseUnitRepository.GetTotalAcademicProgressPercentage(courseSyllabusId, ignoredId);
    }
}