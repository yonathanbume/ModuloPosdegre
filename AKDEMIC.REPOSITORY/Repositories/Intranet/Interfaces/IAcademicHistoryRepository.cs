using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAcademicHistoryRepository : IRepository<AcademicHistory>
    {
        Task<List<AcademicHistoryTemplate>> GetAcademicHistoryTemplate(Guid careerId, Guid curriculumId, Guid courseId, Guid termId);
        Task<AcademicHistory> GetWithData(Guid id);
        Task<AcademicHistory> GetLastByStudentAndCourse(Guid studentId, Guid courseId);
        Task<IEnumerable<AcademicHistory>> GetAllWithSectionAndTeacherSections(Guid? studentId = null, Guid? termId = null, bool? validated = null, bool? approved = null);
        Task<IEnumerable<AcademicHistory>> GetAcademicHistoriesByStudent(Guid studentId, Guid? termId = null, bool? validated = null, bool? approved = null);
        Task<int> CountFailedAcademicHistoriesByStudent(Guid studentId);
        Task<IEnumerable<AcademicHistoryReportTemplate>> GetAcademicHistoriesReportAsData(Guid careerId,Guid curriculumId,Guid courseId, Guid termId);
        Task<DataTablesStructs.ReturnedData<AcademicHistoryTemplate>> GetAcademicHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId,Guid curriculumId,Guid courseId, Guid termId, string name);
        Task<IEnumerable<CurriculumCourseTemplate>> GetCurriculumCoursesByStudentId(Guid studentId, bool electiveCourses);
        Task<List<CertificateTemplate>> GetListCertificateByStudentAndCurriculum(Guid studentId, Guid curriculumId);
        Task<int> GetCoursesDisapprovedByStudentId(Guid studentId);
        Task<int> GetCoursesRecoveredByStudentId(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourth(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null);
        Task<List<FourthExcelTemplate>> GetStudentToExcelFourth(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<List<ThridExcelTemplate>> GetStudentToExcelThird(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<List<AcademicHistory>> GetByStudentId(Guid studentId);
        Task<AcademicHistory> GetApprovedsNyCourseIdAndStudentId(Guid courseId, Guid studentid);
        Task<bool> AnyDisaprovedAcademicHistories(Guid courseId, Guid studentId);
        Task<byte> GetLowestAcademicYear(Guid studentId, Guid curriculumId);
        Task<int> GetDissaprovedCount(Guid studentId);
        Task<bool> IsTryHigherThan(Guid studentId, int number);
        Task<string> GetTimesByCourseAndUserId(Guid courseId, string userId);
        Task<List<AcademicHistory>> GetApprovedByStudentId(Guid studentId);
        Task UpdateAcademicHistoriesJob(string connectionString);
        Task UpdateAcademicHistoryTryJob(string code);
        Task StudentsCurriculumConvalidationJob(Guid careerId);
        Task StudentsCurriculumConvalidationJob(Guid careerId,Guid prevCurriculumId);
        Task<AcademicHistory> GetByStudentIdAndCourseId(Guid studentId, Guid courseId);
        Task<List<AcademicHistory>> GetAcademicHistoryValidated(Guid studentId, Guid termId);
        Task<AcademicHistory> GetAcademicHistoryByStudentAndSectionId(Guid studentId, Guid sectionId);
        Task<AcademicHistory> GetAcademicHistoryBystudentAndCourseId(Guid studentId, Guid courseId, Guid termId);
        Task<Select2Structs.ResponseParameters> GetTAcademicHistoryByCourseAndTerm(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? courseId = null, Guid? termId = null);
        Task<List<AcademicHistoryYearTemplate>> GetAcademicHistoriesByYearRange(int startYear, int endYear);
        Task<List<TermDisapprovedGradeTemplate>> GetDisapprovedAcademicHistoriesByYearRange(int startYear, int endYear);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseRecognitionReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid term, Guid? faculty = null, Guid? career = null, string search = null);
        Task<List<StudentHistoryTemplate>> GetCourseRecognitionReportData(Guid term, Guid? faculty = null, Guid? career = null, string search = null);
        Task<object> GetDisapprovedCoursesByStudentDataDatatable(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null);
        Task<List<DisapprovedCoursesByStudentTemplate>> GetDisapprovedCoursesByStudentData(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null);
        Task<List<CourseEquivalenceTemplate>> GetCourseEquivalenceDataByStudent(Guid studentId, Guid? termId = null);
    }
}
