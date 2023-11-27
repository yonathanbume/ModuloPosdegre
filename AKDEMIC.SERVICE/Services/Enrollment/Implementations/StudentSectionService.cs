using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class StudentSectionService : IStudentSectionService
    {
        private readonly IStudentSectionRepository _studentSectionRepository;
        public StudentSectionService(IStudentSectionRepository studentSectionRepository)
        {
            _studentSectionRepository = studentSectionRepository;
        }

        public async Task<List<Tuple<string, string>>> GetAllStudentsBySection(Guid sectionId)
            => await _studentSectionRepository.GetAllStudentsBySection(sectionId);

        public async Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExam(Guid termId, Guid courseId, Guid studentId)
        {
            return await _studentSectionRepository.AnyByTermIdCourseIdStudentIdAndValidSubstituteExam(termId, courseId, studentId);
        }
        public async Task<bool> AnyByTermIdCourseIdStudentIdAndValidSubstituteExamV2(Guid termId, Guid studentSectionId, Guid studentId)
        {
            return await _studentSectionRepository.AnyByTermIdCourseIdStudentIdAndValidSubstituteExamV2(termId, studentSectionId, studentId);
        }
        public async Task<object> GetAllAsModelA(Guid? studentId = null, Guid? termId = null, bool acta = false)
            => await _studentSectionRepository.GetAllAsModelA(studentId, termId, acta);

        public async Task<DataTablesStructs.ReturnedData<StudentSectionTemplate>> GetStudentSectionDatatable(DataTablesStructs.SentParameters parameters, Guid sid, Guid pid, string search)
        {
            return await _studentSectionRepository.GetStudentSectionDatatable(parameters, sid, pid, search);
        }

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsByStudentId(Guid studentId)
            => await _studentSectionRepository.GetStudentSectionsByStudentId(studentId);

        public async Task Update(StudentSection entity)
            => await _studentSectionRepository.Update(entity);

        public async Task UpdateRange(IEnumerable<StudentSection> entities)
           => await _studentSectionRepository.UpdateRange(entities);

        public async Task Delete(StudentSection studentSection)
            => await _studentSectionRepository.Delete(studentSection);

        public async Task DeleteById(Guid id)
            => await _studentSectionRepository.DeleteById(id);

        public async Task<StudentSection> Get(Guid id)
            => await _studentSectionRepository.Get(id);

        public async Task<IEnumerable<StudentSection>> GetAll(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null)
            => await _studentSectionRepository.GetAll(studentId, termId, sectionId);

        public async Task Insert(StudentSection studentSection)
            => await _studentSectionRepository.Insert(studentSection);

        public async Task InsertSeveral(List<StudentSection> studentSection)
            => await _studentSectionRepository.AddRange(studentSection);
        public async Task<IEnumerable<StudentSection>> GetAllWithGradesAndEvaluations(Guid? studentId = null, Guid? termId = null, Guid? sectionId = null, string teacherId = null)
            => await _studentSectionRepository.GetAllWithGradesAndEvaluations(studentId, termId, sectionId, teacherId);

        public async Task<IEnumerable<CourseTemplate>> GetGradesByStudentIdAndTermId(Guid studentId, Guid termId)
            => await _studentSectionRepository.GetGradesByStudentIdAndTermId(studentId, termId);

        public async Task<DataTablesStructs.ReturnedData<CourseReportCardTemplate>> GetCourseReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termId, string search)
            => await _studentSectionRepository.GetCourseReportCardDatatable(parameters, studentId, termId, search);


        public async Task<DataTablesStructs.ReturnedData<ReportCourseDetailTemplate>> GetReportCourseDetailDataTable(DataTablesStructs.SentParameters parameters, Guid ctid)
        {
            return await _studentSectionRepository.GetReportCourseDetailDataTable(parameters, ctid);
        }

        public async Task<StudentSection> GetByStudentAndCourseTerm(Guid studentId, Guid courseTermId)
            => await _studentSectionRepository.GetByStudentAndCourseTerm(studentId, courseTermId);

        public async Task<List<SelectListItem>> GetTermSelectListByStudent(Guid id)
            => await _studentSectionRepository.GetTermSelectListByStudent(id);

        public async Task<List<CourseTemplate>> GetGradesByStudentAnTerm(Guid studentId, Guid termid)
            => await _studentSectionRepository.GetGradesByStudentAnTerm(studentId, termid);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceByStudentAndTermDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid)
            => await _studentSectionRepository.GetCoursesAssistanceByStudentAndTermDatatable(parameters, studentId, termid);

        public async Task<List<StudentSection>> GetAllBySectionIdWithIncludes(Guid sectionId)
        {
            return await _studentSectionRepository.GetAllBySectionIdWithIncludes(sectionId);
        }

        public async Task<List<StudentSection>> GetAllByCourse(Guid courseId)
            => await _studentSectionRepository.GetAllByCourse(courseId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesReportCardDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid termid)
            => await _studentSectionRepository.GetCoursesReportCardDatatable(parameters, studentId, termid);

        public async Task<decimal> GetTotalCreditsByStudentAndTerm(Guid studentId, Guid? termId = null)
            => await _studentSectionRepository.GetTotalCreditsByStudentAndTerm(studentId, termId);

        public async Task<List<StudentSection>> GetAllSectionStudentsWithUserBySectionId(Guid sectionId)
            => await _studentSectionRepository.GetAllSectionStudentsWithUserBySectionId(sectionId);

        public async Task<object> GetStudentSectionDatatableClientSide(Guid studentId, Guid termId)
            => await _studentSectionRepository.GetStudentSectionDatatableClientSide(studentId, termId);

        public Task<Tuple<bool, string>> ConfirmEnrollment(Guid studentId, Guid termId, string userId)
            => _studentSectionRepository.ConfirmEnrollment(studentId, termId, userId);
        public Task<Tuple<bool, string>> ConfirmEnrollmentRectification(Guid studentId, Guid termId, string userId, string fileName, string fileUrl, string observations = null)
            => _studentSectionRepository.ConfirmEnrollmentRectification(studentId, termId, userId, fileName, fileUrl, observations);

        public Task<object> GetCoursesSelect2ClientSide(Guid studentId, Guid termId, bool filterWithdraw = false)
            => _studentSectionRepository.GetCoursesSelect2ClientSide(studentId, termId, filterWithdraw);

        public async Task<bool> IsEnabledForWithdrawal(Guid studentSectionId)
            => await _studentSectionRepository.IsEnabledForWithdrawal(studentSectionId);

        public async Task DeleteRange(List<StudentSection> studentSection)
            => await _studentSectionRepository.DeleteRange(studentSection);

        public async Task<object> GetStudnetSectionDetail(Guid id, Guid? termId = null)
            => await _studentSectionRepository.GetStudnetSectionDetail(id , termId);

        public async Task<decimal> GetUsedCredits(Guid studentId, int status, Guid courseTermId)
            => await _studentSectionRepository.GetUsedCredits(studentId, status, courseTermId);
        public async Task<bool> GetIntersection(Guid studentId, int status, Guid courseTermId, ICollection<ClassSchedule> classSchedules)
            => await _studentSectionRepository.GetIntersection(studentId, status, courseTermId, classSchedules);

        public async Task<List<StudentSection>> GetActiveSections(Guid courseId, Guid studentId)
            => await _studentSectionRepository.GetActiveSections(courseId, studentId);

        public async Task<IEnumerable<StudentSection>> GetAllWithData()
            => await _studentSectionRepository.GetAllWithData();

        public async Task<List<StudentSection>> GetAllWithDataByStudentIdAndTermId(Guid termId, Guid studentId, bool? isDirectedCourse = false)
            => await _studentSectionRepository.GetAllWithDataByStudentIdAndTermId(termId, studentId, isDirectedCourse);

        public async Task<List<StudentSection>> GetAllWithDataByCareerAndStatus(Guid careerId, int status)
            => await _studentSectionRepository.GetAllWithDataByCareerAndStatus(careerId, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            return await _studentSectionRepository.GetStudentEnrolledReportDatatable(sentParameters, sex, careerId, termId, user);
        }

        public async Task<object> GetStudentEnrolledReportChart(int sex, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            return await _studentSectionRepository.GetStudentEnrolledReportChart(sex, careerId, termId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentEnrolledByInstitutionReportDatatable(DataTablesStructs.SentParameters sentParameters, int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            return await _studentSectionRepository.GetStudentEnrolledByInstitutionReportDatatable(sentParameters, sex, originSchool, careerId, termId, user);
        }

        public async Task<object> GetStudentEnrolledByInstitutionReportChart(int sex, string originSchool = null, Guid? careerId = null, Guid? termId = null, ClaimsPrincipal user = null)
        {
            return await _studentSectionRepository.GetStudentEnrolledByInstitutionReportChart(sex, originSchool, careerId, termId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> curriculums, Guid careerId, Guid termId)
        {
            return await _studentSectionRepository.GetEnrolledStudentsByCurriculumDatatable(sentParameters,curriculums,careerId,termId);
        }

        public async Task<List<CurriculumEnrolledStudentsTemplate>> GetEnrolledStudentsByCurriculum(List<Guid> curriculums, Guid termId)
        {
            return await _studentSectionRepository.GetEnrolledStudentsByCurriculum(curriculums, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentByEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid evaluationReportId, string searchValue = null)
        {
            return await _studentSectionRepository.GetStudentByEvaluationReportDatatable(sentParameters, evaluationReportId, searchValue);
        }

        public async Task<EnrolledByGroupsDataTemplate> GetEnrolledStudentsByGroupData(Guid termId, Guid careerId, Guid curriculums)
        {
            return await _studentSectionRepository.GetEnrolledStudentsByGroupData(termId, careerId, curriculums);
        }

        public async Task<DataTablesStructs.ReturnedData<CourseModalityTemplate>> GetEnrolledStudentsByModalityDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId)
        {
            return await _studentSectionRepository.GetEnrolledStudentsByModalityDatatable(sentParameters, termId, careerId);
        }

        public async Task<List<CourseModalityTemplate>> GetEnrolledStudentsByModalityData(Guid termId, Guid careerId)
        {
            return await _studentSectionRepository.GetEnrolledStudentsByModalityData(termId, careerId);
        }

        public async  Task<object> GetEnrolledTrends()
        {
            return await _studentSectionRepository.GetEnrolledTrends();
        }

        public async Task UpdateStudentSectionFinalGradeJob()
        {
            await _studentSectionRepository.UpdateStudentSectionFinalGradeJob();
        }

        public async Task<bool> AnyBySectionAndStudentId(Guid sectionId, Guid studentId)
        {
            return await _studentSectionRepository.AnyBySectionAndStudentId(sectionId, studentId);
        }

        public async Task<bool> IsStudentEnabledForCourse(Guid sectionId, Guid studentId)
            => await _studentSectionRepository.IsStudentEnabledForCourse(sectionId, studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledSectionsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null, bool? withStudentsInProcess = null)
            => await _studentSectionRepository.GetEnrolledSectionsByTermDatatable(sentParameters, termId, facultyId, careerId, academicProgramId,curriculumId ,searchValue,user, academicYear, withStudentsInProcess);
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null)
            => await _studentSectionRepository.GetEnrolledCoursesByTermDatatable(sentParameters, termId, facultyId, careerId, academicProgramId, curriculumId, searchValue, user, academicYear);
        public async Task<List<EnrolledCourseTermTemplate>> GetEnrolledCoursesByTermData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? curriculumId = null, string searchValue = null, ClaimsPrincipal user = null, byte? academicYear = null)
            => await _studentSectionRepository.GetEnrolledCoursesByTermData(termId, facultyId, careerId, academicProgramId, curriculumId, searchValue, user, academicYear);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId,string searchValue = null, int? studentSectionStatus = null)
            => await _studentSectionRepository.GetEnrolledStudentsBySectionDatatable(sentParameters,sectionId, searchValue, studentSectionStatus);
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCourseDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, string searchValue = null)
         => await _studentSectionRepository.GetEnrolledStudentsByCourseDatatable(sentParameters, sectionId, searchValue);
        public async Task<object> GetSummerEnrolledBySection(Guid sectionId)
            => await _studentSectionRepository.GetSummerEnrolledBySection(sectionId);

        public async Task DeleteIfEnrolledInAnotherSection(Guid sectionId, Guid studentId)
            => await _studentSectionRepository.DeleteIfEnrolledInAnotherSection(sectionId, studentId);

        public async Task<List<Student>> GetEnrolledStudentsBySection(Guid sectionId)
            => await _studentSectionRepository.GetEnrolledStudentsBySection(sectionId);
        public async Task<List<StudentSection>> GetEnrolledStudentsByCourse(Guid courseTermId)
            => await _studentSectionRepository.GetEnrolledStudentsByCourse(courseTermId);
        public async Task UpdateStudentSectionFinalGrade(List<Guid> sectionStudents,Guid termid)
        {
            await _studentSectionRepository.UpdateStudentSectionFinalGrade(sectionStudents,termid);
        }

        public async Task<Tuple<bool, string>> InsertWithValidations(StudentSection studentSection)
            => await _studentSectionRepository.InsertWithValidations(studentSection);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsWithSectionGroupDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, Guid? sectionGroupId = null, string searchValue = null)
            => await _studentSectionRepository.GetStudentSectionsWithSectionGroupDatatable(sentParameters, sectionId, sectionGroupId, searchValue);

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsBySectionId(Guid sectionId)
            => await _studentSectionRepository.GetStudentSectionsBySectionId(sectionId);

        public async Task GroupAlphabeticallyInSectionGroups(Guid sectionId)
            => await _studentSectionRepository.GroupAlphabeticallyInSectionGroups(sectionId);

        public async Task<IEnumerable<StudentSection>> GetStudentSectionsByTeacherId(Guid sectionId, string teacherId,Guid? studentId = null)
            => await _studentSectionRepository.GetStudentSectionsByTeacherId(sectionId, teacherId, studentId);

        public async Task<DataTablesStructs.ReturnedData<StudentAssistanceReportTemplate>> GetStudentAssistanceReportDataTable(Guid sectionId)
            => await _studentSectionRepository.GetStudentAssistanceReportDataTable(sectionId);

        public async Task<StudentGradeReportTemplate> GetGradesByCourses(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? academicProgramId, Guid? courseId, byte? academicYear = null)
            => await _studentSectionRepository.GetGradesByCourses(termId, facultyId, careerId, curriculumId, academicProgramId, courseId, academicYear);

        public async Task<object> GetStudentsWithParallelCoursesDatatable(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsWithParallelCoursesDatatable(termId, facultyId, careerId, user);

        public async Task<object> GetStudentParallelCoursesDatatable(Guid termId, Guid studentId)
            => await _studentSectionRepository.GetStudentParallelCoursesDatatable(termId, studentId);
        public async Task<List<StudentSection>> GetByCareerIdAndTermId(Guid careerId, Guid termId)
            => await _studentSectionRepository.GetByCareerIdAndTermId(careerId, termId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetIrregularStudents(DataTablesStructs.SentParameters sentParameters, int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null)
            => await _studentSectionRepository.GetIrregularStudents(sentParameters, time, search, career, user, academicyear);
        public async Task<List<StudentSectionIrregularTutoringReportTemplate>> GetIrregularStudentsExport(int? time = null, string search = null, Guid? career = null, ClaimsPrincipal user = null, int? academicyear = null)
            => await _studentSectionRepository.GetIrregularStudentsExport(time, search, career, user, academicyear);
        public async Task<int> CountDirectedCourseAttempts(Guid studentId, Guid courseId)
            => await _studentSectionRepository.CountDirectedCourseAttempts(studentId, courseId);

        public async Task<object> GetDirectedCourseStudentsDatatableClientSide(Guid courseId, Guid termId)
            => await _studentSectionRepository.GetDirectedCourseStudentsDatatableClientSide(courseId, termId);

        public async Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculumId)
            => await _studentSectionRepository.GetEnrollmentDirectedCourseData(termId, careerId, curriculumId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCourseStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, Guid? courseId = null, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetDirectedCourseStudentsDataDatatable(sentParameters, termId, careerId, facultyId, courseId, searchValue, user);

        public async Task<IEnumerable<Student>> GetStudentsByCourseTermId(Guid courseTermId)
            => await _studentSectionRepository.GetStudentsByCourseTermId(courseTermId);

        public async Task DeleteRangeWithData(List<StudentSection> studentSections)
            => await _studentSectionRepository.DeleteRangeWithData(studentSections);

        public async Task RecalculateFinalGrade(Guid studentSectionId)
            => await _studentSectionRepository.RecalculateFinalGrade(studentSectionId);

        public async Task RecalculateFinalGradeByCourseTerm(Guid courseTermId)
            => await _studentSectionRepository.RecalculateFinalGradeByCourseTerm(courseTermId);

        public async Task RecalculateFinalGradeByCourseSyllabusId(Guid courseSyllabusId)
            => await _studentSectionRepository.RecalculateFinalGradeByCourseSyllabusId(courseSyllabusId);

        public async Task<object> GetSectionStudentsSelect2(Guid sectionId)
            => await _studentSectionRepository.GetSectionStudentsSelect2(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetFinalGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId)
            => await _studentSectionRepository.GetFinalGradeReportDatatable(sentParameters, sectionId);

        public async Task<object> GetReportCardHeader(Guid studentId, Guid termid)
        {
            return await _studentSectionRepository.GetReportCardHeader(studentId, termid);
        }

        public async Task<bool> HasStudentSections(Guid studentId, Guid termId)
            => await _studentSectionRepository.HasStudentSections(studentId, termId);

        public async Task<object> GetGradesByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId)
         => await _studentSectionRepository.GetGradesByCompetences(termId, facultyId, careerId, curriculumId, competenceId);

        public async Task<List<DataReport2>> AchievementLevelCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId)
          => await _studentSectionRepository.AchievementLevelCompetences(termId, facultyId, careerId, curriculumId);

        public async Task<object> AchievementLevelCompetenceDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, List<RangeLevel> rangeLevelList)
         => await _studentSectionRepository.AchievementLevelCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, rangeLevelList);

        public async Task<object> AchievementLevelCompetenceStudentDetail(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, Guid? competenceId, Guid? studentId, List<RangeLevel> rangeLevelList)
        => await _studentSectionRepository.AchievementLevelCompetenceStudentDetail(termId, facultyId, careerId, curriculumId, competenceId, studentId, rangeLevelList);

        public async Task<object> GetReportAchievementLevel(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
            => await _studentSectionRepository.GetReportAchievementLevel(termId, facultyId, careerId, curriculumId);

        public async Task<object> GetStudentsEnrolledByTermChart(Guid? termId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsEnrolledByTermChart(termId, careerId, academicProgramId, user);

        public async Task<SectionPartialGradesReportTemplate> GetSectionPartialGradesTemplate(Guid sectionId, Guid? studentId = null)
            => await _studentSectionRepository.GetSectionPartialGradesTemplate(sectionId, studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsThirdEnrollmentDatatable(sentParameters, termId, facultyId, careerId, search, user);

        public async Task<List<ThridExcelTemplate>> GetStudentsThirdEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsThirdEnrollmentData(termId, facultyId, careerId, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourthEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsFourthEnrollmentDatatable(sentParameters, termId, facultyId, careerId, search, user);

        public async Task<List<FourthExcelTemplate>> GetStudentsFourthEnrollmentData(Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsFourthEnrollmentData(termId, facultyId, careerId, search, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetSectionsByCareerDatatable(sentParameters, termId, careerId, user);

        public Task<object> GetSectionsByCareerChart(Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetSectionsByCareerChart(termId, careerId, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetSectionsByFacultyDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetSectionsByFacultyDatatable(sentParameters, termId, facultyId, user);

        public Task<object> GetSectionsByFacultyChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetSectionsByFacultyChart(termId, facultyId, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentsPerformanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetStudentsPerformanceDatatable(sentParameters, termId, user);

        public Task<object> GetStudentsPerformanceChart(Guid? termId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetStudentsPerformanceChart(termId, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentPartenStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
             => _studentSectionRepository.GetStudentPartenStudyLevelDatatable(sentParameters, termId, user);

        public Task<object> GetStudentPartenStudyLevelChart(Guid? termId = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetStudentPartenStudyLevelChart(termId, user);

        public async Task RecalculateFinalGradeBySectionId(Guid sectionId)
            => await _studentSectionRepository.RecalculateFinalGradeBySectionId(sectionId);

        public async Task<List<EnrolledStudentTemplate>> GetEnrolledStudentWithOutInstitutionalEmailData(Guid termId)
            => await _studentSectionRepository.GetEnrolledStudentWithOutInstitutionalEmailData(termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConditionedStudentSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null)
            => await _studentSectionRepository.GetConditionedStudentSectionsDatatable(sentParameters, termId, careerId, conditionType, search);
        
        public async Task<List<ConditionedStudentSectionTemplate>> GetConditionedStudentSectionsData(Guid? termId = null, Guid? careerId = null, byte? conditionType = null, string search = null)
            => await _studentSectionRepository.GetConditionedStudentSectionsData(termId, careerId, conditionType, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentWithdrawnDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentWithdrawnDatatable(sentParameters, termId, facultyId, careerId, courseId, searchValue, user);
        public async Task<List<StudentWithdrawnTemplate>> GetStudentWithdrawnData(Guid termId, Guid? facultyId, Guid? careerId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentWithdrawnData(termId, facultyId, careerId, courseId, searchValue, user);
        public async Task<EnrollmentReportTemplate> GetEnrollmentReportTemplate(Guid studentId, Guid? termId = null, bool? pronabec = false, string qrUrl = null, bool? includeExtraordinaryEvaluations = false, bool? isExtraordinaryReport = false)
            => await _studentSectionRepository.GetEnrollmentReportTemplate(studentId, termId, pronabec, qrUrl, includeExtraordinaryEvaluations, isExtraordinaryReport);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsApprovedDisapprovedBySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsApprovedDisapprovedBySectionDatatable(sentParameters, termId, careerId, curriculumId, year, user);
        
        public async Task<List<SectionApprovedDataTemplate>> GetStudentsApprovedDisapprovedBySectionData(Guid termId, Guid careerId, Guid curriculumId, int? year = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetStudentsApprovedDisapprovedBySectionData(termId, careerId, curriculumId, year, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicPerformanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetAcademicPerformanceReportDatatable(sentParameters, termId, facultyId, careerId, user);

        public async Task<List<CareerAcademicPerformanceTemplate>> GetAcademicPerformanceReportData(Guid termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentSectionRepository.GetAcademicPerformanceReportData(termId, facultyId, careerId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsDetailedReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null)
            => await _studentSectionRepository.GetDisapprovedStudentsDetailedReportDatatable(sentParameters, termid, facultyId, careerId, user, studentTry);

        public async Task<List<DisapprovedStudentSectionTemplate>> GetDisapprovedStudentsDetailedReportData(Guid termid, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, byte? studentTry = null)
            => await _studentSectionRepository.GetDisapprovedStudentsDetailedReportData(termid, facultyId, careerId, user, studentTry);

        public async Task<List<StudentSectionRegisterGradeTemplate>> GetStudentSectionsRegisterGradeTemplate(Guid sectionId, string teacherId = null)
            => await _studentSectionRepository.GetStudentSectionsRegisterGradeTemplate(sectionId, teacherId);

        public async Task<List<StudentSection>> GetByStudentAndTerm(Guid termId, Guid studentId)
            => await _studentSectionRepository.GetByStudentAndTerm(termId, studentId);

        public async Task<double> GetStudentHigherAbsencePercentage(Guid studentId, Guid termId)
            => await _studentSectionRepository.GetStudentHigherAbsencePercentage(studentId, termId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentSectionsIntranetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null)
            => _studentSectionRepository.GetStudentSectionsIntranetReportDatatable(sentParameters, termId, careerId, academicYear, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudentsByUnitReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? curriculumId, byte? academicYear, byte unit)
            => await _studentSectionRepository.GetDisapprovedStudentsByUnitReportDatatable(sentParameters, careerId, curriculumId, academicYear, unit);

        public async Task<List<StudentGradeTemplate>> GetStudentGradesTemplate(Guid studentSectionId)
            => await _studentSectionRepository.GetStudentGradesTemplate(studentSectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentAveragePerUnitDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid careerId, int unitNumber, string search)
            => await _studentSectionRepository.GetStudentAveragePerUnitDatatable(sentParameters, termId, careerId, unitNumber, search);
    }
}