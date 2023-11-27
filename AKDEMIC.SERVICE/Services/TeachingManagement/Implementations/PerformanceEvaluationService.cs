using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationService : IPerformanceEvaluationService
    {
        private readonly IPerformanceEvaluationRepository _repository;

        public PerformanceEvaluationService(IPerformanceEvaluationRepository repository)
        {
            _repository = repository;
        }
        public async Task<PerformanceEvaluation> Get(Guid id)
        {
            return await _repository.Get(id);
        }
        public async Task<object> GetPerformanceEvaluation(Guid id)
        {
            return await _repository.GetPerformanceEvaluation(id);
        }
        public async Task<bool> AnyPerformanceEvaluationByTag(string code, string name, Guid? id)
        {
            return await _repository.AnyPerformanceEvaluationByTag(code, name, id);
        }
        public async Task<IEnumerable<object>> GetRoles(string userId)
        {
            return await _repository.GetRoles(userId);
        }
        public async Task<ReturnedData<object>> GetPerformanceEvaluationsDatatable(SentParameters sentParameters, string searchValue = null)
        {
            return await _repository.GetPerformanceEvaluationsDatatable(sentParameters, searchValue);
        }
        public async Task<ReturnedData<object>> GetTeachersByStudentDatatable(SentParameters sentParameters, string userId)
        {
            return await _repository.GetTeachersByStudentDatatable(sentParameters, userId);
        }
        public async Task<ReturnedData<object>> GetUsersDatatable(SentParameters sentParameters, string userId, string roleId)
        {
            return await _repository.GetUsersDatatable(sentParameters, userId, roleId);
        }
        public async Task<object> GetEvaluatorInfo(string userId, string role)
        {
            return await _repository.GetEvaluatorInfo(userId, role);
        }
        public async Task DeleteById(Guid id)
        {
            await _repository.DeleteById(id);
        }
        public async Task Insert(PerformanceEvaluation entity)
        {
            await _repository.Insert(entity);
        }
        public async Task Update(PerformanceEvaluation entity)
        {
            await _repository.Update(entity);
        }

        public async Task<DetailedReport> GetDetailedReportChartJS(Guid evaluationId, Guid academicDepartmentId)
            => await _repository.GetDetailedReportChartJS(evaluationId, academicDepartmentId);

        public async Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, Guid academicDepartmentId)
            => await _repository.GetCommentariesByEvaluation(evaluationId, academicDepartmentId);

        public async Task<int> GetPendingSurveys(string userId)
            => await _repository.GetPendingSurveys(userId);

        public async Task<CommentaryReport> GetCommentariesByEvaluation(Guid evaluationId, ClaimsPrincipal user)
            => await _repository.GetCommentariesByEvaluation(evaluationId, user);

        public async Task<List<string>> GetCommentariesByTeacher(Guid evaluationId, string teacherId)
            => await _repository.GetCommentariesByTeacher(evaluationId, teacherId);

        public async Task<bool> OnlyViewStudentPerformanceEvaluation(string userId)
            => await _repository.OnlyViewStudentPerformanceEvaluation(userId);

        public async Task<string> GetSummaryStudentsByEvaluation(Guid evaluationId)
            => await _repository.GetSummaryStudentsByEvaluation(evaluationId);

        public async Task<bool> AnyPerformanceEvaluationByDateRange(Guid termId, DateTime startDate, DateTime endDate, Guid? ignoredId)
            => await _repository.AnyPerformanceEvaluationByDateRange(termId, startDate, endDate, ignoredId);

        public async Task<PerformanceEvaluation> GetPerformanceEvaluationInCourseByTerm()
            => await _repository.GetPerformanceEvaluationInCourseByTerm();

        public async Task<bool> EvaluationHasResponses(Guid evaluationId)
            => await _repository.EvaluationHasResponses(evaluationId);

        public async Task DeleteRelatedPerformanceEvaluation(Guid evaluationId)
            => await _repository.DeleteRelatedPerformanceEvaluation(evaluationId);

        public async Task<ReportSectionConsolidatedTemplate> GetReportSectionConsolidated(Guid evaluationId, Guid? academicDepartmentId)
            => await _repository.GetReportSectionConsolidated(evaluationId, academicDepartmentId);

        public async Task<int> GenerateResultScale(Guid performanceEvaluationId)
            => await _repository.GenerateResultScale(performanceEvaluationId);

        public async Task<List<DetailedReportTemplate>> GetDetailedReport(Guid performanceEvaluationId)
            => await _repository.GetDetailedReport(performanceEvaluationId);

        public async Task<string> GetNewCode(Guid termId)
            => await _repository.GetNewCode(termId);
    }
}
