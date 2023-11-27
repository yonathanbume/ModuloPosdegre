using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAcademicYearCourseService
    {
        Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZById(Guid curriculumId, Guid? academicProgramId = null);
        Task<IEnumerable<AcademicYearCourseTemplateZ>> GetAllAsModelZ(Guid curriculumId, string academicProgramCode);
        Task<int?> GetLevelByCourseAndCurriculum(Guid courseId, Guid? curriculumId = null);
        Task<IEnumerable<object>> GetCurriculumCoursesSelect2(Guid curriculumId);
        Task<IEnumerable<AcademicYearCourse>> GetAllByCurriculumWithPrerequisites(Guid curriculumId);
        Task<IEnumerable<AcademicYearCourse>> GetAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null);
        Task<IEnumerable<AcademicYearCourseTemplateA>> GetAllAsModelA(Guid? curriculumId = null);
        Task<int> CountAll(Guid? curriculumId = null, byte? academicYear = null, bool? isElective = null, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicHistoryV2CoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
        Task<List<AcademicHistoryCourseTemplate>> GetAcademicHistoryV2Courses(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumElectivesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesRecognitionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetPendingCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId);
        Task<object> GetCoursesByCurriculumActive();
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumCourses2Datatable(DataTablesStructs.SentParameters sentParameters, Guid careerId,Guid? academicProgramId, Guid curriculumId, int? cycle, Guid? termId, string groupCode, ClaimsPrincipal user = null);
        Task<List<CurriculumTemplate>> GetCurriculumsByCareerId(Guid id);
        Task<IEnumerable<Tuple<Guid, string>>> GetTuplesCourseIdAndAcademicYear();
        Task<List<PendingCoursesTemplate>> GetPendingCoursesStudent(Guid studentId, Guid curriculumId);
        Task<List<CurriculumTemplate>> GetCurriculumDetail(Guid curriculumId);
        Task<AcademicYearCourse> GetAcademicYearCourseByCourseId(Guid courseId);
        Task<List<AcademicYearCourse>> GetAllAcademicYearCoursesByCourseId(Guid courseId);
        Task<object> GetCoursesByCareerIdAndCurriculumActive(Guid careerId);
        Task<List<AcademicYearCourseDetail>> GetAcademicYearDetailByStudentIdAndAcademicYearId(Guid studentId, byte academicYear, ClaimsPrincipal user = null);
        Task<object> GetAcademicSituationElectiveByStudentId(Guid studentId);
        Task<object> GetAcademicSummaryDetail(Guid studentId, Guid termId);
        Task<AcademicYearCourse> GetAsync(Guid id);
        Task InsertAsync(AcademicYearCourse academicYearCourse);
        Task InsertRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses);
        Task DeleteAsync(AcademicYearCourse academicYearCourse);
        Task DeleteRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses);
        Task UpdateAsync(AcademicYearCourse academicYearCourse);
        Task UpdateRangeAsync(IEnumerable<AcademicYearCourse> academicYearCourses);
        Task<bool> AnyByIdAndCourseId(Guid id, Guid courseId);
        Task<bool> AnyByCourseIdAndAndCurriculumId(Guid courseId, Guid curriculumId);
        Task<object> GetAllAsSelect2ClientSide(Guid? curriculumId = null, string name = null, Guid? onlycourseId = null, bool? onlyOptional = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAvailableCoursesStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null);
        Task<AcademicYearCourse> GetWithCourseAndCurriculum(Guid courseId, Guid curriculumId);

        Task<List<AcademicYearCourse>> GetCourseEnrollment(Guid careerId, byte academicYear);
        Task<object> GetStudentCourse(Guid careerId, Guid studenId, Guid? termId = null);

        Task<List<Guid>> GetCourseId(Guid careerId,Guid curriculumId);
        Task<object> GetCoursesByCurriculum(Guid curriculumId, byte? academicYear = null);
        Task<int> GetCourseCount(Guid id, int number);
        Task<IEnumerable<AcademicYearCourse>> GetAllWithData();
        Task<string> GetCourseEquivalences(Guid studentId, Guid courseId);
        Task<List<AcademicYearCourse>> GetWithDataByCurriculumIdAndAcademic(Guid curriculumId, int academicYear, int academicYearDispersion, IEnumerable<AcademicHistory> academicHistories);
        Task<AcademicYearCourse> GetWithData(Guid id);

        Task<object> GetCareerAcademicYear(Guid id);
        Task<object> GetCurriculumAcademicYearsJson(Guid curriculumId, ClaimsPrincipal user = null);
        Task<object> GetCurriculumAcademicYearsAsSelect2(Guid? careerId = null, Guid? curriculumId = null, bool isActive = false, string coordinatorId = null);
        Task<object> GetStudentCourses(Guid id, string userId, int currentAcademicYear, Guid curriculumId);
        Task<object> GetCurriculumCoursesDatatableClientSide(Guid curriculumId, Guid? academicProgramId = null, Guid? competencieId = null);
        Task<object> GetCurriculumCourseTermsDatatableClientSide(Guid curriculumId, Guid? termId = null);
        Task<IEnumerable<CourseTermTemplate>> GetCurriculumCourseTermsAsModelA(Guid curriculumId, Guid? termId = null);

        Task<IEnumerable<AcademicYearCourse>> GetWithHistoriesByCurriculumAndStudent(Guid curriculumId, Guid studentId);
        Task<object> GetAllAcademicYearByCareerId(Guid careerId);
        Task<List<AcademicProgramByCurriculumTemplate>> GetAcademicProgramByCurriculum(Guid curriculumId , Guid? academicProgramId = null);
        Task<object> GetCareerAcademicProgramsByCurriculumSelect2(Guid curriculumId);
        Task<List<byte>> GetAcademicYearsByCurriculumId(Guid curriculumId);
        Task<List<AcademicYearCourse>> GetAcademicYearsByCurriculumIdAndAcademicYear(Guid curriculumId, byte academicYear);
        Task UpdateAcademicYearCoursesJob();
        Task<AcademicYearCourse> GetFirstByCurriculumAndCode(Guid curriculumId, string courseCode);

        Task<object> GetDataDetailById(Guid id, string groupCode, Guid? termId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesToManageSyllabus(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, int? cycle = null, ClaimsPrincipal user = null, string searchValue = null, byte? status = null);
        Task<int> GetMaxAcademic(Guid curriculinId);
        Task<object> GetAllSelect2();
        Task<List<CourseSyllabusCompliance>> GetCoursesToManageSyllabusCompliance(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, byte? status);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentCurriculumCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<List<AcademicYearCourse>> GetAcademicYearCoursesByCurriculumId(Guid curriculumId);
        Task<string> GetNextCorrelative(Guid curriculumId);
    }
}