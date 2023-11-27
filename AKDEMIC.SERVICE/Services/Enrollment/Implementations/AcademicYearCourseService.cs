using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AcademicYearCourseService : IAcademicYearCourseService
    {
        private readonly IAcademicYearCourseRepository _academicYearCourseRepository;

        public AcademicYearCourseService(IAcademicYearCourseRepository academicYearCourseRepository)
        {
            _academicYearCourseRepository = academicYearCourseRepository;
        }

        public async Task<IEnumerable<AcademicYearCourse>> GetAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null)
        {
            return await _academicYearCourseRepository.GetAll(curriculumId, academicYear, isElective, academicProgramId);
        }

        public async Task<int> CountAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null)
        {
            return await _academicYearCourseRepository.CountAll(curriculumId, academicYear, isElective, academicProgramId);
        }

        public async Task<IEnumerable<AcademicYearCourse>> GetAllByCurriculumWithPrerequisites(Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetAllByCurriculumWithPrerequisites(curriculumId);
        }

        public async Task<IEnumerable<object>> GetCurriculumCoursesSelect2(Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetCurriculumCoursesSelect2(curriculumId);
        }

        public async Task<int?> GetLevelByCourseAndCurriculum(Guid courseId, Guid? curriculumId = null)
        {
            return await _academicYearCourseRepository.GetLevelByCourseAndCurriculum(courseId, curriculumId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            return await _academicYearCourseRepository.GetCurriculumCoursesDatatable(sentParameters, id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumElectivesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            return await _academicYearCourseRepository.GetCurriculumElectivesDatatable(sentParameters, id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPendingCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            return await _academicYearCourseRepository.GetPendingCoursesStudentDatatable(sentParameters, studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetCurriculumCourses2Datatable(sentParameters, curriculumId);
        }

        public async Task<IEnumerable<Tuple<Guid, string>>> GetTuplesCourseIdAndAcademicYear()
        {
            return await _academicYearCourseRepository.GetTuplesCourseIdAndAcademicYear();
        }

        public async Task<List<PendingCoursesTemplate>> GetPendingCoursesStudent(Guid studentId, Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetPendingCoursesStudent(studentId, curriculumId);
        }

        public async Task<List<CurriculumTemplate>> GetCurriculumDetail(Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetCurriculumDetail(curriculumId);
        }

        public async Task<AcademicYearCourse> GetAcademicYearCourseByCourseId(Guid courseId)
        {
            return await _academicYearCourseRepository.GetAcademicYearCourseByCourseId(courseId);
        }

        public async Task<object> GetCoursesByCareerIdAndCurriculumActive(Guid careerId)
        {
            return await _academicYearCourseRepository.GetCoursesByCareerIdAndCurriculumActive(careerId);
        }

        public async Task<List<AcademicYearCourseDetail>> GetAcademicYearDetailByStudentIdAndAcademicYearId(Guid studentId, byte academicYear, ClaimsPrincipal user = null)
        {
            return await _academicYearCourseRepository.GetAcademicYearDetailByStudentIdAndAcademicYearId(studentId, academicYear, user);
        }

        public async Task<object> GetAcademicSituationElectiveByStudentId(Guid studentId)
        {
            return await _academicYearCourseRepository.GetAcademicSituationElectiveByStudentId(studentId);
        }

        public async Task<object> GetAcademicSummaryDetail(Guid studentId, Guid termId)
        {
            return await _academicYearCourseRepository.GetAcademicSummaryDetail(studentId, termId);
        }

        public Task<AcademicYearCourse> GetAsync(Guid id)
        {
            return _academicYearCourseRepository.Get(id);
        }

        public Task InsertAsync(AcademicYearCourse academicYearCourse)
        {
            return _academicYearCourseRepository.Insert(academicYearCourse);
        }

        public Task InsertRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses)
        {
            return _academicYearCourseRepository.InsertRange(academicYearCourses);
        }

        public Task DeleteAsync(AcademicYearCourse academicYearCourse)
        {
            return _academicYearCourseRepository.Delete(academicYearCourse);
        }

        public Task DeleteRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses)
        {
            return _academicYearCourseRepository.DeleteRange(academicYearCourses);
        }

        public Task UpdateAsync(AcademicYearCourse academicYearCourse)
        {
            return _academicYearCourseRepository.Update(academicYearCourse);
        }

        public Task UpdateRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses)
        {
            return _academicYearCourseRepository.UpdateRange(academicYearCourses);
        }

        public Task<bool> AnyByIdAndCourseId(Guid id, Guid courseId)
        {
            return _academicYearCourseRepository.AnyByIdAndCourseId(id, courseId);
        }

        public Task<bool> AnyByCourseIdAndAndCurriculumId(Guid courseId, Guid curriculumId)
        {
            return _academicYearCourseRepository.AnyByCourseIdAndAndCurriculumId(courseId, curriculumId);
        }

        public Task<IEnumerable<AcademicYearCourseTemplateA>> GetAllAsModelA(Guid? curriculumId = null)
        {
            return _academicYearCourseRepository.GetAllAsModelA(curriculumId);
        }

        public Task<object> GetAllAsSelect2ClientSide(Guid? curriculumId = null, string name = null, Guid? onlycourseId = null, bool? onlyOptional = null)
        {
            return _academicYearCourseRepository.GetAllAsSelect2ClientSide(curriculumId, name, onlycourseId, onlyOptional);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetAvailableCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null)
        {
            return _academicYearCourseRepository.GetAvailableCoursesStudentDatatable(sentParameters, studentId, termId);
        }

        public async Task<AcademicYearCourse> GetWithCourseAndCurriculum(Guid courseId, Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetWithCourseAndCurriculum(courseId, curriculumId);
        }

        public async Task<List<AcademicYearCourse>> GetCourseEnrollment(Guid careerId, byte academicYear)
        {
            return await _academicYearCourseRepository.GetCourseEnrollment(careerId, academicYear);
        }

        public async Task<object> GetStudentCourse(Guid careerId, Guid studenId, Guid? termId = null)
        {
            return await _academicYearCourseRepository.GetStudentCourse(careerId, studenId, termId);
        }

        public async Task<List<Guid>> GetCourseId(Guid careerId, Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetCourseId(careerId, curriculumId);
        }

        public async Task<int> GetCourseCount(Guid id, int number)
        {
            return await _academicYearCourseRepository.GetCourseCount(id, number);
        }

        public async Task<IEnumerable<AcademicYearCourse>> GetAllWithData()
        {
            return await _academicYearCourseRepository.GetAllWithData();
        }

        public async Task<string> GetCourseEquivalences(Guid studentId, Guid courseId)
        {
            return await _academicYearCourseRepository.GetCourseEquivalences(studentId, courseId);
        }

        public async Task<List<AcademicYearCourse>> GetWithDataByCurriculumIdAndAcademic(Guid curriculumId, int academicYear, int academicYearDispersion, IEnumerable<AcademicHistory> academicHistories)
        {
            return await _academicYearCourseRepository.GetWithDataByCurriculumIdAndAcademic(curriculumId, academicYear, academicYearDispersion, academicHistories);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicHistoryV2CoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            return await _academicYearCourseRepository.GetAcademicHistoryV2CoursesDatatable(sentParameters, studentId);
        }

        public async Task<List<AcademicHistoryCourseTemplate>> GetAcademicHistoryV2Courses(Guid studentId)
            => await _academicYearCourseRepository.GetAcademicHistoryV2Courses(studentId);
        
        public async Task<object> GetCoursesByCurriculum(Guid curriculumId, byte? academicYear = null)
            => await _academicYearCourseRepository.GetCoursesByCurriculum(curriculumId, academicYear);

        public async Task<AcademicYearCourse> GetWithData(Guid id)
        {
            return await _academicYearCourseRepository.GetWithData(id);
        }

        public async Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZ(Guid curriculumId, string academicProgramCode)
        {
            return await _academicYearCourseRepository.GetAllAsModelZ(curriculumId, academicProgramCode);
        }

        public async Task<object> GetCareerAcademicYear(Guid id)
        {
            return await _academicYearCourseRepository.GetCareerAcademicYear(id);
        }

        public async Task<object> GetCurriculumAcademicYearsJson(Guid curriculumId , ClaimsPrincipal user = null)
        {
            return await _academicYearCourseRepository.GetCurriculumAcademicYearsJson(curriculumId, user);
        }

        public async Task<object> GetCurriculumAcademicYearsAsSelect2(Guid? careerId = null, Guid? curriculumId = null, bool isActive = false, string coordinatorId = null)
        {
            return await _academicYearCourseRepository.GetCurriculumAcademicYearsAsSelect2(careerId, curriculumId, isActive, coordinatorId);
        }

        public async Task<object> GetCurriculumCoursesDatatableClientSide(Guid curriculumId, Guid? academicProgramId = null, Guid? competencieId = null)
        {
            return await _academicYearCourseRepository.GetCurriculumCoursesDatatableClientSide(curriculumId, academicProgramId, competencieId);
        }

        public async Task<IEnumerable<AcademicYearCourse>> GetWithHistoriesByCurriculumAndStudent(Guid curriculumId, Guid studentId)
        {
            return await _academicYearCourseRepository.GetWithHistoriesByCurriculumAndStudent(curriculumId, studentId);
        }

        public async Task<object> GetAllAcademicYearByCareerId(Guid careerId)
        {
            return await _academicYearCourseRepository.GetAllAcademicYearByCareerId(careerId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid careerId, Guid? academicProgramId, Guid curriculumId, int? cycle, Guid? termId, string groupCode, ClaimsPrincipal user = null)
        {
            return await _academicYearCourseRepository.GetCurriculumCourses2Datatable(sentParameters, careerId, academicProgramId, curriculumId, cycle, termId, groupCode, user);
        }

        public async Task<List<AcademicProgramByCurriculumTemplate>> GetAcademicProgramByCurriculum(Guid curriculumId, Guid? academicProgramId = null)
        {
            return await _academicYearCourseRepository.GetAcademicProgramByCurriculum(curriculumId);
        }

        public async Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZById(Guid curriculumId, Guid? academicProgramId = null)
        {
            return await _academicYearCourseRepository.GetAllAsModelZById(curriculumId, academicProgramId);
        }

        public async Task<object> GetCoursesByCurriculumActive()
        {
            return await _academicYearCourseRepository.GetCoursesByCurriculumActive();
        }

        public async Task<List<CurriculumTemplate>> GetCurriculumsByCareerId(Guid id)
        {
            return await _academicYearCourseRepository.GetCurriculumsByCareerId(id);
        }

        public async Task<List<byte>> GetAcademicYearsByCurriculumId(Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetAcademicYearsByCurriculumId(curriculumId);
        }

        public async Task<List<AcademicYearCourse>> GetAcademicYearsByCurriculumIdAndAcademicYear(Guid curriculumId, byte academicYear)
        {
            return await _academicYearCourseRepository.GetAcademicYearsByCurriculumIdAndAcademicYear(curriculumId, academicYear);
        }

        public async Task<object> GetStudentCourses(Guid id, string userId, int currentAcademicYear, Guid curriculumId)
        {
            return await _academicYearCourseRepository.GetStudentCourses(id, userId, currentAcademicYear, curriculumId);
        }

        public async Task UpdateAcademicYearCoursesJob()
        {
            await _academicYearCourseRepository.UpdateAcademicYearCoursesJob();
        }

        public async Task<AcademicYearCourse> GetFirstByCurriculumAndCode(Guid curriculumId, string courseCode)
        {
            return await _academicYearCourseRepository.GetFirstByCurriculumAndCode(curriculumId, courseCode);
        }

        public async Task<object> GetDataDetailById(Guid id, string groupCode, Guid? termId = null)
        {
            return await _academicYearCourseRepository.GetDataDetailById(id, groupCode, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesToManageSyllabus(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, int? cycle = null, ClaimsPrincipal user = null, string searchValue = null, byte? status = null)
            => await _academicYearCourseRepository.GetCoursesToManageSyllabus(parameters, termId, careerId, academicProgramId, curriculumId, cycle, user, searchValue, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesRecognitionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            return await _academicYearCourseRepository.GetCoursesRecognitionStudentDatatable(sentParameters, studentId);
        }

        public async Task<object> GetCareerAcademicProgramsByCurriculumSelect2(Guid curriculumId) 
            => await _academicYearCourseRepository.GetCareerAcademicProgramsByCurriculumSelect2(curriculumId);

        public async Task<List<AcademicYearCourse>> GetAllAcademicYearCoursesByCourseId(Guid courseId)
            => await _academicYearCourseRepository.GetAllAcademicYearCoursesByCourseId(courseId);

        public async Task<object> GetCurriculumCourseTermsDatatableClientSide(Guid curriculumId, Guid? termId = null)
            => await _academicYearCourseRepository.GetCurriculumCourseTermsDatatableClientSide(curriculumId, termId);

        public async Task<IEnumerable<CourseTermTemplate>> GetCurriculumCourseTermsAsModelA(Guid curriculumId, Guid? termId = null)
            => await _academicYearCourseRepository.GetCurriculumCourseTermsAsModelA(curriculumId, termId);
        public async Task<int> GetMaxAcademic(Guid curriculinId)
            => await _academicYearCourseRepository.GetMaxAcademic(curriculinId);
        public async Task<object> GetAllSelect2()
            => await _academicYearCourseRepository.GetAllSelect2();

        public async Task<List<CourseSyllabusCompliance>> GetCoursesToManageSyllabusCompliance(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, byte? status)
            => await _academicYearCourseRepository.GetCoursesToManageSyllabusCompliance(termId, careerId, academicProgramId, curriculumId, cycle, user, status);
    
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _academicYearCourseRepository.GetEnrollmentCurriculumCoursesDatatable(sentParameters, id);

        public async Task<List<AcademicYearCourse>> GetAcademicYearCoursesByCurriculumId(Guid curriculumId)
            => await _academicYearCourseRepository.GetAcademicYearCoursesByCurriculumId(curriculumId);

        public async Task<string> GetNextCorrelative(Guid curriculumId)
            => await _academicYearCourseRepository.GetNextCorrelative(curriculumId);
    }
}