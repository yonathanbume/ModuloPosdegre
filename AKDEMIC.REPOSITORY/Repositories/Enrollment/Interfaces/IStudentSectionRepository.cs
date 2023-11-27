using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IStudentSectionRepository : IRepository<StudentSection>
    {
        Task<List<Tuple<string, string>>> GetAllStudentsBySection(Guid sectionId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentAveragePerUnitDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, int unitNumber, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetFinalGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId);
        Task<List<Student>> GetEnrolledStudentsBySection(Guid sectionId);
        Task<List<StudentGradeTemplate>> GetStudentGradesTemplate(Guid studentSectionId);
        Task<List<StudentSection>> GetEnrolledStudentsByCourse(Guid courseTermId);
        Task<DataTablesStructs.ReturnedData<StudentSectionTemplate>> GetStudentSectionDatatable(DataTablesStructs.SentParameters parameters, Guid sid, Guid pid, string search);
        Task<List<StudentSectionRegisterGradeTemplate>> GetStudentSectionsRegisterGradeTemplate(Guid sectionId, string teacherId = null);
        Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExam(Guid termId, Guid courseId, Guid studentId);
        Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExamV2(Guid termId, Guid studentSectionId, Guid studentId);
        Task<decimal> GetTotalCreditsByStudentAndTerm(Guid studentId, Guid? termId = null);
        Task<object> GetAllAsModelA(Guid? studentId = null, Guid? termId = null, bool acta = false);
        Task<StudentSection> GetByStudentAndCourseTerm(Guid studentId, Guid courseTermId);
        Task<IEnumerable<StudentSection>> GetAll(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null);
        Task<IEnumerable<StudentSection>> GetStudentSectionsByStudentId(Guid studentId);
        Task<object> GetCoursesSelect2ClientSide(Guid studentId, Guid termId, bool filterWithdraw = false);
        Task<IEnumerable<CourseTemplate>> GetGradesByStudentIdAndTermId(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<CourseReportCardTemplate>> GetCourseReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termId, string search);
        Task<IEnumerable<StudentSection>> GetAllWithGradesAndEvaluations(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null, string teacherId = null);
        Task<DataTablesStructs.ReturnedData<ReportCourseDetailTemplate>> GetReportCourseDetailDataTable(DataTablesStructs.SentParameters parameters, Guid ctid);
        Task<List<SelectListItem>> GetTermSelectListByStudent(Guid id);
        Task<List<CourseTemplate>> GetGradesByStudentAnTerm(Guid studentId, Guid termid);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceByStudentAndTermDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid);
        Task<List<StudentSection>> GetAllBySectionIdWithIncludes(Guid sectionId);
        Task<List<StudentSection>> GetAllByCourse(Guid courseId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledSectionsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null , Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null, bool? withStudentsInProcess = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null , Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null);
        Task<List<EnrolledCourseTermTemplate>> GetEnrolledCoursesByTermData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, string searchValue = null, int? studentSectionStatus = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCourseDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsByFacultyDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<object> GetSectionsByCareerChart(Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<object> GetSectionsByFacultyChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsPerformanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetStudentsPerformanceChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentPartenStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetStudentPartenStudyLevelChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid);
        Task<List<StudentSection>> GetAllSectionStudentsWithUserBySectionId(Guid sectionId);
        Task<object> GetStudentSectionDatatableClientSide(Guid studentId, Guid termId);
        Task<Tuple<bool, string>> ConfirmEnrollment(Guid studentId, Guid termId, string userId);
        Task<Tuple<bool, string>> ConfirmEnrollmentRectification(Guid studentId, Guid termId, string userId, string fileName = null, string fileUrl = null, string observations = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledByInstitutionReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> curriculums, Guid careerId, Guid termId);
        Task<List<CurriculumEnrolledStudentsTemplate>> GetEnrolledStudentsByCurriculum(List<Guid> curriculums, Guid termId);
        Task<object> GetStudentEnrolledReportChart(int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetStudentEnrolledByInstitutionReportChart(int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null);
        Task CourseWithdrawal(Guid studentId, Guid termId, Guid courseId);
        Task<bool> IsEnabledForWithdrawal(Guid studentSectionId);
        Task<bool> HasStudentSections(Guid studentId, Guid termId);
        Task<object> GetStudnetSectionDetail(Guid id, Guid? termId = null);
        Task<decimal> GetUsedCredits(Guid studentId, int status, Guid courseTermId);
        Task<bool> GetIntersection(Guid studentId, int status, Guid courseTermId, ICollection<ClassSchedule> classSchedules);
        Task<List<StudentSection>> GetActiveSections(Guid courseId, Guid studentId);
        Task<IEnumerable<StudentSection>> GetAllWithData();
        Task<List<StudentSection>> GetAllWithDataByStudentIdAndTermId(Guid termId, Guid studentId, bool? isDirectedCourse = false);
        Task<List<StudentSection>> GetAllWithDataByCareerAndStatus(Guid careerId, int status);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentByEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid evaluationReportId, string searchValue = null);
        Task<EnrolledByGroupsDataTemplate> GetEnrolledStudentsByGroupData(Guid termId, Guid careerId, Guid curriculums);
        //Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByGroupDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, Guid curriculums);
        Task<DataTablesStructs.ReturnedData<CourseModalityTemplate>> GetEnrolledStudentsByModalityDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId);
        Task<List<CourseModalityTemplate>> GetEnrolledStudentsByModalityData(Guid termId, Guid careerId);
        Task<object> GetEnrolledTrends();
        Task UpdateStudentSectionFinalGradeJob();
        Task<bool> AnyBySectionAndStudentId(Guid sectionId, Guid studentId);

        Task<bool> IsStudentEnabledForCourse(Guid sectionId, Guid studentId);
        Task<object> GetSummerEnrolledBySection(Guid sectionId);

        Task DeleteIfEnrolledInAnotherSection(Guid sectionId, Guid studentId);

        Task<Tuple<bool, string>> InsertWithValidations(StudentSection studentSection);
        Task UpdateStudentSectionFinalGrade(List<Guid> sectionStudents,Guid termid);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsWithSectionGroupDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, Guid? sectionGroupId = null, string searchValue = null);
        Task<IEnumerable<StudentSection>> GetStudentSectionsBySectionId(Guid sectionId);
        Task GroupAlphabeticallyInSectionGroups(Guid sectionId);
        Task<IEnumerable<StudentSection>> GetStudentSectionsByTeacherId(Guid sectionId, string teacherId,Guid? studentId = null);
        Task<DataTablesStructs.ReturnedData<StudentAssistanceReportTemplate>> GetStudentAssistanceReportDataTable(Guid sectionId);

        Task<StudentGradeReportTemplate> GetGradesByCourses(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? academicProgramId, Guid? courseId, byte? academicYear = null);
        Task<object> GetStudentsWithParallelCoursesDatatable(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<object> GetStudentParallelCoursesDatatable(Guid termId, Guid studentId);
        Task<List<StudentSection>> GetByCareerIdAndTermId(Guid careerId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetIrregularStudents(DataTablesStructs.SentParameters sentParameters, int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null);
        Task<List<StudentSectionIrregularTutoringReportTemplate>> GetIrregularStudentsExport(int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null);
        Task<int> CountDirectedCourseAttempts(Guid studentId, Guid courseId);
        Task<object> GetDirectedCourseStudentsDatatableClientSide(Guid courseId, Guid termId);

        Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetDirectedCourseStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters,
            Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, Guid? courseId = null, string searchValue = null, ClaimsPrincipal user = null);

        Task<IEnumerable<Student>> GetStudentsByCourseTermId(Guid courseTermId);
        Task DeleteRangeWithData(List<StudentSection> studentSections);
        Task RecalculateFinalGrade(Guid studentSectionId);
        Task RecalculateFinalGradeByCourseTerm(Guid courseTermId);
        Task RecalculateFinalGradeByCourseSyllabusId(Guid courseSyllabusId);
        Task RecalculateFinalGradeBySectionId(Guid sectionId);
        Task<object> GetSectionStudentsSelect2(Guid sectionId);
        Task<object> GetReportCardHeader(Guid studentId, Guid termid);
        Task<object> GetGradesByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId);
        Task<List<DataReport2>> AchievementLevelCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId);
        Task<object> AchievementLevelCompetenceDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, List<RangeLevel> rangeLevelList);
        Task<object> AchievementLevelCompetenceStudentDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, Guid? studentId, List<RangeLevel> rangeLevelList);
        Task<object> GetReportAchievementLevel(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId);
        Task<object> GetStudentsEnrolledByTermChart(Guid? termId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null);
        Task<SectionPartialGradesReportTemplate> GetSectionPartialGradesTemplate(Guid sectionId, Guid? studentId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<List<ThridExcelTemplate>> GetStudentsThirdEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<List<EnrolledStudentTemplate>> GetEnrolledStudentWithOutInstitutionalEmailData(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourthEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<List<FourthExcelTemplate>> GetStudentsFourthEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<List<ConditionedStudentSectionTemplate>> GetConditionedStudentSectionsData(Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetConditionedStudentSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentWithdrawnDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null);
        Task<List<StudentWithdrawnTemplate>> GetStudentWithdrawnData(Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null);
        Task<EnrollmentReportTemplate> GetEnrollmentReportTemplate(Guid studentId, Guid? termId = null, bool? pronabec = false, string qrUrl = null, bool? includeExtraordinaryEvaluations = false, bool? isExtraordinaryReport = false);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsApprovedDisapprovedBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsIntranetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null);
        Task<List<SectionApprovedDataTemplate>> GetStudentsApprovedDisapprovedBySectionData(Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAcademicPerformanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<List<CareerAcademicPerformanceTemplate>> GetAcademicPerformanceReportData(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsDetailedReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null);
        Task<List<DisapprovedStudentSectionTemplate>> GetDisapprovedStudentsDetailedReportData(Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null);
        Task<List<StudentSection>> GetByStudentAndTerm(Guid termId, Guid studentId/*, bool? isDirectedCourse = false*/);
        Task<double> GetStudentHigherAbsencePercentage(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsByUnitReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? curriculumId, byte? academicYear, byte unit);
    }
}
