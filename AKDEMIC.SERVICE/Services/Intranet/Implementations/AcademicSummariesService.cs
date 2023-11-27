using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AcademicSummariesService : IAcademicSummariesService
    {
        private readonly IAcademicSummariesRepository _academicSummaryRepository;

        public AcademicSummariesService(IAcademicSummariesRepository academicSummaryRepository)
        {
            _academicSummaryRepository = academicSummaryRepository;
        }

        public async Task<IEnumerable<AcademicSummaryReportTemplate>> GetAcademicSummariesReportAsData(Guid? careerId, Guid termId)
        {
            return await _academicSummaryRepository.GetAcademicSummariesReportAsDataAsync(careerId, termId);
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllByStudent(Guid studentId)
        {
            return await _academicSummaryRepository.GetAllByStudent(studentId);
        }

        public async Task<AcademicSummary> GetByStudentAndTerm(Guid studentId, Guid termId)
        {
            return await _academicSummaryRepository.GetByStudentAndTerm(studentId, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryTemplate>> GetAcademicSummaryDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid termId, string name)
        {
            return await _academicSummaryRepository.GetAcademicSummaryDatatable(parameters, careerId, termId, name);
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllByStudentOrderedByTermDesc(Guid studentId)
        {
            return await _academicSummaryRepository.GetAllByStudentOrderedByTermDesc(studentId);
        }
        public async Task<List<MeritChartDetailTemplate>> GetDetailMeritChart(Guid studentId)
        {
            return await _academicSummaryRepository.GetDetailMeritChart(studentId);
        }

        public async Task<decimal> GetAverage(Guid? graduationTermid = null)
        {
            return await _academicSummaryRepository.GetAverage(graduationTermid);
        }

        public async Task<decimal> GetCurrent(Guid studentId, Guid? graduationTermid = null)
        {
            return await _academicSummaryRepository.GetCurrent(studentId, graduationTermid);
        }

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailUpperFith(Guid studentId)
        {
            return await _academicSummaryRepository.GetDetailUpperFith(studentId);
        }

        public async Task<decimal> GetAverageBachelorsDegree(Guid studentId, Guid? graduationTermid = null)
        {
            return await _academicSummaryRepository.GetAverageBachelorsDegree(studentId, graduationTermid);
        }

        public async Task<int> GetacademicSemestersCount(Guid studentId)
        {
            return await _academicSummaryRepository.GetacademicSemestersCount(studentId);
        }

        public async Task<decimal> GetCurrentWeightedGrade(Guid studentId, Guid termId)
        {
            return await _academicSummaryRepository.GetCurrentWeightedGrade(studentId, termId);
        }

        public async Task<IEnumerable<AcademicSummary>> GetAllWithIncludesByStudent(Guid studentId)
        {
            return await _academicSummaryRepository.GetAllWithIncludesByStudent(studentId);
        }

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailUpperThird(Guid studentId)
        {
            return await _academicSummaryRepository.GetDetailUpperThird(studentId);
        }

        public async Task<AcademicSummary> GetLastTermSanctionedByStudentId(Guid studentId)
        {
            return await _academicSummaryRepository.GetLastTermSanctionedByStudentId(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetGraduatedInTimeDatatable(sentParameters, user);
        }

        public async Task<object> GetGraduatedInTimeChart(ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetGraduatedInTimeChart(user);
        }
        public async Task<List<AcademicPerformanceSummaryTemplate>> GetAcademicPerformanceSummary(Guid studentId)
        {
            return await _academicSummaryRepository.GetAcademicPerformanceSummary(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetAcademicSummariesByTermIdDatatable(sentParameters, termId, user);
        }

        public async Task<AcademicSummary> GetAcademicSumariesByStudentTermAndCareer(Guid studentId, Guid careerId, Guid termId)
        {
            return await _academicSummaryRepository.GetAcademicSumariesByStudentTermAndCareer(studentId, careerId, termId);
        }

        public async Task<int> GetCountByCareerIdAndTermId(Guid careerId, Guid termId)
        {
            return await _academicSummaryRepository.GetCountByCareerIdAndTermId(careerId, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAverageGradesByConditionsDatatable(DataTablesStructs.SentParameters sentParameters, int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetAverageGradesByConditionsDatatable(sentParameters, sex, termId, careerId, user);
        }

        public async Task<object> GetAverageGradesByConditionsChart(int sex, Guid? termId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetAverageGradesByConditionsChart(sex, termId, careerId, user);
        }

        public async Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByTermIdData(Guid termId, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetAcademicSummariesByTermIdData(termId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user = null, Guid? facultyId = null)
        {
            return await _academicSummaryRepository.GetAcademicSummariesByCycleDatatable(sentParameters, termId, user, facultyId);
        }

        public async Task<List<AcademicSummaryByCareerTemplate>> GetAcademicSummariesByCycleData(Guid termId, ClaimsPrincipal user, Guid? facultyId = null)
        {
            return await _academicSummaryRepository.GetAcademicSummariesByCycleData(termId, user, facultyId);
        }

        public async Task<decimal> GetStudentAverageAcumulative(Guid studentId, Guid? termId = null)
            => await _academicSummaryRepository.GetStudentAverageAcumulative(studentId, termId);

        public async Task UpdateAcademicSummariesJob()
        {
            await _academicSummaryRepository.UpdateAcademicSummariesJob();
        }

        public async Task CreateAcademicSummariesJob(string connectionString, string careerCode)
        {
            await _academicSummaryRepository.CreateAcademicSummariesJob(connectionString, careerCode);
        }

        public async Task ReCreateStudentAcademicSummaries(Guid studentId)
           => await _academicSummaryRepository.ReCreateStudentAcademicSummaries(studentId);

        public async Task CreateAcademicSummariesOldJob()
        {
            await _academicSummaryRepository.CreateAcademicSummariesOldJob();
        }
        public async Task UpdateAcademicSummariesMeritOrderJob(string connectionString)
        {
            await _academicSummaryRepository.UpdateAcademicSummariesMeritOrderJob(connectionString);
        }

        public async Task UpdateAcademicSummariesOrderJob()
        {
            await _academicSummaryRepository.UpdateAcademicSummariesOrderJob();
        }

        public async Task UpdateAcademicSummariesOrder2Job()
        {
            await _academicSummaryRepository.UpdateAcademicSummariesOrder2Job();
        }

        public async Task<object> CalculateMeritOrder2Job(Guid termId, Guid curriculumId, int academicYear)
        {
            return await _academicSummaryRepository.CalculateMeritOrder2Job(termId, curriculumId, academicYear);
        }

        public async Task<object> CalculateMeritOrderJob(Guid termId, Guid curriculumId, int academicYear)
        {
            return await _academicSummaryRepository.CalculateMeritOrderJob(termId, curriculumId, academicYear);
        }

        public async Task UpdateAcademicSummariesYearJob(Guid termId, string careerCode)
        {
            await _academicSummaryRepository.UpdateAcademicSummariesYearJob(termId, careerCode);
        }

        public async Task UpdateAcademicSummariesYearJob(Guid termId)
        {
            await _academicSummaryRepository.UpdateAcademicSummariesYearJob(termId);
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetDisapprovedStudents(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, string search)
        {
            return await _academicSummaryRepository.GetDisapprovedStudents(sentParameters, termId, careerId, facultyId, search);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetTermsByStudent(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user)
        {
            return await _academicSummaryRepository.GetTermsByStudent(sentParameters, careerId, facultyId, search, user);
        }

        public async Task<decimal> GetTotalCreditsApproved(Guid studentId)
            => await _academicSummaryRepository.GetTotalCreditsApproved(studentId);

        public async Task<List<AcademicSummary>> GetMeritChartAcademicSummaries(Guid termId, Guid facultyId, Guid careerId, Guid? curriculumId = null, int? year = null, int? academicOrder = null, ClaimsPrincipal user = null)
        {
            return await _academicSummaryRepository.GetMeritChartAcademicSummaries(termId, facultyId, careerId, curriculumId, year, academicOrder, user);
        }

        public async Task<List<RowChild4>> GetReportStudentByCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear)
        {
            return await _academicSummaryRepository.GetReportStudentByCompetences(termId, facultyId, careerId, curriculumId, academicYear);
        }

        public async Task<object> ReportGradesByStudentCourses(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear)
        {
            return await _academicSummaryRepository.ReportGradesByStudentCourses(termId, facultyId, careerId, curriculumId, competenceId, studentId, academicYear);
        }

        public async Task<List<DataReport2>> AchievementLevelAcademicYearCompetences(Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, byte academicYear)
        {
            return await _academicSummaryRepository.AchievementLevelAcademicYearCompetences(termId, facultyId, careerId, curriculumId, academicYear);
        }

        public async Task<object> AchievementLevelAcademicYearCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, byte academicYear, List<RangeLevel> rangeLevelList)
        {
            return await _academicSummaryRepository.AchievementLevelAcademicYearCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, academicYear, rangeLevelList);
        }

        public async Task<object> AchievementLevelAcademicYearStudentCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear, List<RangeLevel> rangeLevelList)
        {
            return await _academicSummaryRepository.AchievementLevelAcademicYearStudentCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, studentId, academicYear, rangeLevelList);
        }

        public async Task<object> GetReportAchievementLevelAcademicYear(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            return await _academicSummaryRepository.GetReportAchievementLevelAcademicYear(termId, facultyId, careerId, curriculumId, academicYear);
        }

        public async Task<List<DisapprovedStudentsByTermTemplate>> GetDisapprrovedStudentsByTerm(int startYear, int endYear)
            => await _academicSummaryRepository.GetDisapprrovedStudentsByTerm(startYear, endYear);

        public async Task<StudentDistributionByTimeTemplate> StudentDistributionByEnrolledTime(Guid termId)
            => await _academicSummaryRepository.StudentDistributionByEnrolledTime(termId);

        public async Task<object> GetAcademicSummariesReportByStudent(Guid studentId)
            => await _academicSummaryRepository.GetAcademicSummariesReportByStudent(studentId);

        public async Task<List<AcademicSummaryTemplate>> GetAcademicSummaryTemplate(Guid? careerId, Guid? termId)
            => await _academicSummaryRepository.GetAcademicSummaryTemplate(careerId, termId);

        public async Task<List<DisapprovedStudentsTemplate>> GetDisapprovedStudentsTemplate(Guid? termId, Guid? careerId, Guid? facultyId)
            => await _academicSummaryRepository.GetDisapprovedStudentsTemplate(termId, careerId, facultyId);

        public async Task<List<TermByStudentTemplate>> GetTermsByStudentTemplate(Guid? careerId, Guid? facultyId, string search, ClaimsPrincipal user)
            => await _academicSummaryRepository.GetTermsByStudentTemplate(careerId, facultyId, search, user);

        public async Task<List<UpperFifthDetailsTemplate>> GetDetailTenthFith(Guid studentId)
            => await _academicSummaryRepository.GetDetailTenthFith(studentId);
    }
}
