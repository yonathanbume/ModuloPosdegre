using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationService
    {
        Task<PerformanceEvaluation> Get(Guid id);
        Task DeleteRelatedPerformanceEvaluation(Guid evaluationId);
        Task<object> GetPerformanceEvaluation(Guid id);
        Task<bool> AnyPerformanceEvaluationByTag(string code, string name, Guid? id);
        Task<IEnumerable<object>> GetRoles(string userId);
        Task<ReturnedData<object>> GetPerformanceEvaluationsDatatable(SentParameters sentParameters, string searchValue = null);
        Task<ReturnedData<object>> GetTeachersByStudentDatatable(SentParameters sentParameters, string userId);
        Task<ReturnedData<object>> GetUsersDatatable(SentParameters sentParameters, string userId, string roleId);
        Task<object> GetEvaluatorInfo(string userId, string role);
        Task DeleteById(Guid id);
        Task Insert(PerformanceEvaluation entity);
        Task Update(PerformanceEvaluation entity);
        Task<DetailedReport> GetDetailedReportChartJS(Guid evaluationId, Guid academicDepartmentId);
        Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, Guid academicDepartmentId);
        Task<int> GetPendingSurveys(string userId);
        Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, ClaimsPrincipal user);
        Task<List<string>> GetCommentariesByTeacher(Guid evaluationId, string teacherId);
        Task<bool> OnlyViewStudentPerformanceEvaluation(string userId);
        Task<string> GetSummaryStudentsByEvaluation(Guid evaluationId);
        Task<bool> AnyPerformanceEvaluationByDateRange(Guid termId, DateTime startDate, DateTime endDate, Guid? ignoredId);
        Task<PerformanceEvaluation> GetPerformanceEvaluationInCourseByTerm();
        Task<bool> EvaluationHasResponses(Guid evaluationId);
        Task<ReportSectionConsolidatedTemplate> GetReportSectionConsolidated(Guid evaluationId, Guid? academicDepartmentId);
        Task<int> GenerateResultScale(Guid performanceEvaluationId);
        Task<List<DetailedReportTemplate>> GetDetailedReport(Guid performanceEvaluationId);
        Task<string> GetNewCode(Guid termId);
    }
}
