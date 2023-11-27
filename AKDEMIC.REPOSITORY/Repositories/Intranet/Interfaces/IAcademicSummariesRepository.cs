using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IAcademicSummariesRepository : IRepository<AcademicSummary>
    {
        Task<List<AcademicSummaryTemplate>> GetAcademicSummaryTemplate(Guid? careerId, Guid? termId);
        Task<DataTablesStructs.ReturnedData<AcademicSummaryTemplate>> GetAcademicSummaryDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid termId, string name);
        Task<DataTablesStructs.ReturnedData<object>> GetGraduatedInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<object> GetGraduatedInTimeChart(ClaimsPrincipal user = null);
        Task<IEnumerable<AcademicSummaryReportTemplate>> GetAcademicSummariesReportAsDataAsync(Guid? careerId, Guid termId);
        Task<AcademicSummary> GetByStudentAndTerm(Guid studentId, Guid termId);
        Task<IEnumerable<AcademicSummary>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<AcademicSummary>> GetAllWithIncludesByStudent(Guid studentId);
        Task<IEnumerable<AcademicSummary>> GetAllByStudentOrderedByTermDesc(Guid studentId);
        Task<List<MeritChartDetailTemplate>> GetDetailMeritChart(Guid studentId);
        Task<decimal> GetAverage(Guid? graduationTermid = null);
        Task<decimal> GetStudentAverageAcumulative(Guid studentId, Guid? termId = null);
        Task<decimal> GetCurrent(Guid studentId, Guid? graduationTermid = null);
        Task<List<TermByStudentTemplate>> GetTermsByStudentTemplate(Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user);
        Task<List<DisapprovedStudentsTemplate>> GetDisapprovedStudentsTemplate(Guid? termId, Guid? careerId, Guid? facultyId);
        Task<List<UpperFifthDetailsTemplate>> GetDetailUpperThird(Guid studentId);
        Task<List<UpperFifthDetailsTemplate>> GetDetailUpperFith(Guid studentId);
        Task<decimal> GetAverageBachelorsDegree(Guid studentId, Guid? graduationTermid = null);
        Task<int> GetacademicSemestersCount(Guid studentId);

        Task<decimal> GetCurrentWeightedGrade(Guid studentId, Guid termId);
        Task<AcademicSummary> GetLastTermSanctionedByStudentId(Guid studentId);
        Task<List<AcademicPerformanceSummaryTemplate>> GetAcademicPerformanceSummary(Guid studentId);
        Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null);
        Task<AcademicSummary> GetAcademicSumariesByStudentTermAndCareer(Guid studentId, Guid careerId, Guid termId);
        Task<int> GetCountByCareerIdAndTermId(Guid careerId, Guid termId);
        Task<decimal> GetTotalCreditsApproved(Guid studentId);

        Task<DataTablesStructs.ReturnedData<object>> GetAverageGradesByConditionsDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<object> GetAverageGradesByConditionsChart(int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdData(Guid termId, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null, Guid? facultyId = null);
        Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleData(Guid termId, ClaimsPrincipal user, Guid? facultyId = null);
        Task UpdateAcademicSummariesJob();
        Task CreateAcademicSummariesJob(string connectionString, string careerCode);
        Task ReCreateStudentAcademicSummaries(Guid studentId);
        Task UpdateAcademicSummariesMeritOrderJob(string connectionString);
        Task CreateAcademicSummariesOldJob();
        Task UpdateAcademicSummariesOrderJob();
        Task UpdateAcademicSummariesOrder2Job();
        Task<object> CalculateMeritOrderJob(Guid termId, Guid curriculumId, int academicYear);
        Task<object> CalculateMeritOrder2Job(Guid termId, Guid curriculumId, int academicYear);
        Task UpdateAcademicSummariesYearJob(Guid termId, string careerCode);
        Task UpdateAcademicSummariesYearJob(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudents(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetTermsByStudent(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user);
        Task<List<AcademicSummary>> GetMeritChartAcademicSummaries(Guid termId, Guid facultyId, Guid careerId, Guid? curriculumId = null, int? year = null, int? academicOrder = null, ClaimsPrincipal user = null);
        Task<List<RowChild4>> GetReportStudentByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear);
        Task<object> ReportGradesByStudentCourses(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear);
        Task<List<DataReport2>> AchievementLevelAcademicYearCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear);
        Task<object> AchievementLevelAcademicYearCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, byte academicYear, List<RangeLevel> rangeLevelList);
        Task<object> AchievementLevelAcademicYearStudentCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear, List<RangeLevel> rangeLevelList);
        Task<object> GetReportAchievementLevelAcademicYear(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear);
        Task<List<DisapprovedStudentsByTermTemplate>> GetDisapprrovedStudentsByTerm(int startYear, int endYear);
        Task<StudentDistributionByTimeTemplate> StudentDistributionByEnrolledTime(Guid termId);
        Task<object> GetAcademicSummariesReportByStudent(Guid studentId);
        Task<List<UpperFifthDetailsTemplate>> GetDetailTenthFith(Guid studentId);
    }
}
