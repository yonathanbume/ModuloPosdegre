namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    //public class AcademicYearService : IAcademicYearService
    //{
    //    private readonly IAcademicYearRepository _academicYearRepository;

    //    public AcademicYearService(IAcademicYearRepository academicYearRepository)
    //    {
    //        _academicYearRepository = academicYearRepository;
    //    }

    //    public Task<bool> AnyByCurriculumIdAndNumber(Guid curriculumId, byte number)
    //        => _academicYearRepository.AnyByCurriculumIdAndNumber(curriculumId, number);

    //    public Task DeleteAsync(AcademicYear academicYear)
    //        => _academicYearRepository.Delete(academicYear);

    //    public Task DeleteRangeAsync(IEnumerable<AcademicYear> academicYears)
    //        => _academicYearRepository.DeleteRange(academicYears);

    //    public Task<IEnumerable<AcademicYearTemplateA>> GetAllAsModelA(Guid? curriculumId = null, string academicProgramCode = null)
    //        => _academicYearRepository.GetAllAsModelA(curriculumId, academicProgramCode);

    //    public Task<object> GetAllAsSelect2(Guid? careerId = null, bool isActive = false, string coordinatorId = null)
    //        => _academicYearRepository.GetAllAsSelect2(careerId, isActive, coordinatorId);

    //    public async Task<IEnumerable<AcademicYear>> GetAllByCurriculum(Guid curriculumId)
    //        => await _academicYearRepository.GetAllByCurriculum(curriculumId);

    //    public async Task<IEnumerable<AcademicYear>> GetAllWithAcademicYearCoursesAndHistoriesByCurriculumAndStudent(Guid curriculumId, Guid studentId)
    //        => await _academicYearRepository.GetAllWithAcademicYearCoursesAndHistoriesByCurriculumAndStudent(curriculumId, studentId);

    //    public async Task<IEnumerable<AcademicYear>> GetAllWithAcademicYearCoursesByCurriculum(Guid curriculumId)
    //        => await _academicYearRepository.GetAllWithAcademicYearCoursesByCurriculum(curriculumId);

    //    public async Task<AcademicYear> GetAsync(Guid id)
    //        => await _academicYearRepository.Get(id);

    //    public Task InsertAsync(AcademicYear academicYear)
    //        => _academicYearRepository.Insert(academicYear);

    //    public Task InsertRangeAsync(IEnumerable<AcademicYear> academicYears)
    //        => _academicYearRepository.InsertRange(academicYears);

    //    public Task UpdateAsync(AcademicYear academicYear)
    //        => _academicYearRepository.Update(academicYear);

    //    public Task UpdateRangeAsync(IEnumerable<AcademicYear> academicYears)
    //        => _academicYearRepository.UpdateRange(academicYears);

    //    public async Task<object> GetCareerAcademicYear(Guid id)
    //        => await _academicYearRepository.GetCareerAcademicYear(id);

    //    public async Task<object> GetAllCareerAcademicYear(Guid id)
    //        => await _academicYearRepository.GetAllCareerAcademicYear(id);
    //    public async Task<object> GetCurriculumAcademicYearJson(Guid id)
    //        => await _academicYearRepository.GetCurriculumAcademicYearJson(id);
    //    public async Task<object> GetAllAcademicYearsByCareer(Guid careerId)
    //        => await _academicYearRepository.GetAllAcademicYearsByCareer(careerId);
    //}
}