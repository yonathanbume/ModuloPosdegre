using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IPerformanceEvaluationUserService
    {
        Task<PerformanceEvaluationUser> Get(Guid id);
        Task<bool> ValidatePerformanceEvaluationUser(string fromRoleId, Guid templateId, string fromId, string toId,Guid? sectionId = null);
        Task<ReturnedData<object>> GetEvaluatedUsers(SentParameters sentParameters, Guid evaluationId, string userId = null);
        Task<PerformanceEvaluationUserDetail> GetResult(Guid templateId, string toUserId, string fromResult, Guid? sectionId = null);
        Task DeleteById(Guid id);
        Task Insert(PerformanceEvaluationUser entity);
        Task Update(PerformanceEvaluationUser entity);
        Task<PerformanceEvaluationUserDetail> GetResultBySection(Guid evaluationId, Guid sectionId, string toUserId);
        Task<PerformanceEvaluationUserReportTemplate> GetEvalutedTeachersDatatableClientSide(Guid evaluationId, Guid academicDepartmentId);
        Task<List<PerformanceEvaluationAuthoritiesTemplate>> GetAuthoritiesComplianceReportDatatableClientSide(Guid evaluationId);
        Task<List<PerformationEvaluationStudentTemplate>> GetStudentComplianceReportDatatableClientSide(Guid evaluationId);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid evaluationId, Guid academicDepartmentId, Guid? curriculumId ,string searchValue);
        Task<StatisticalReportCareerTeacherPerformanceTemplate> GetStatisticalReportCareerTeacherPerformance(Guid evaluationId, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentCompliancecDetailedReportDatatable(DataTablesStructs.SentParameters parameters ,Guid evaluationId, Guid? careerId, string searchValue);
        Task<List<ComplianceDetailedReport>> GetStudentCompliancecDetailedReport(Guid evaluationId, Guid? careerId, string searchvalue);
        Task<List<PerformanceEvaluationCriteronTemplate>> GetTeacherReportPerformanceEvaluationCriterion(Guid? academicDepartmentId, Guid evaluationId);
        Task<StudentDebtResultTemplate> GenerateStudentDebts(Guid evaluationId);
        Task<List<PerformanceEvaluationUserReportTemplate>> GetReportConsolidatedByAcademicDepartment(Guid evaluationId);
        Task<List<Question>> GetReportConsolidatedByQuestion(Guid evaluationId);
    }
}
