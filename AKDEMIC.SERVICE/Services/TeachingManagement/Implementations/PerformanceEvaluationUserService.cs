using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class PerformanceEvaluationUserService : IPerformanceEvaluationUserService
    {
        private readonly IPerformanceEvaluationUserRepository _repository;

        public PerformanceEvaluationUserService(IPerformanceEvaluationUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<PerformanceEvaluationUser> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        public async Task<bool> ValidatePerformanceEvaluationUser(string fromRoleId, Guid templateId, string fromId, string toId, Guid? sectionId = null)
        {
            return await _repository.ValidatePerformanceEvaluationUser(fromRoleId, templateId, fromId, toId, sectionId);
        }

        public async Task<PerformanceEvaluationUserDetail> GetResult(Guid templateId, string toUserId, string fromRoleId, Guid? sectionId = null)
        {
            return await _repository.GetResult(templateId, toUserId, fromRoleId, sectionId);
        }

        public async Task DeleteById(Guid id)
        {
            await _repository.DeleteById(id);
        }

        public async Task Insert(PerformanceEvaluationUser entity)
        {
            await _repository.Insert(entity);
        }

        public async Task Update(PerformanceEvaluationUser entity)
        {
            await _repository.Update(entity);
        }

        public async Task<PerformanceEvaluationUserReportTemplate> GetEvalutedTeachersDatatableClientSide(Guid evaluationId, Guid academicDepartmentId)
            => await _repository.GetEvalutedTeachersDatatableClientSide(evaluationId, academicDepartmentId);

        public async Task<List<PerformanceEvaluationAuthoritiesTemplate>> GetAuthoritiesComplianceReportDatatableClientSide(Guid evaluationId)
            => await _repository.GetAuthoritiesComplianceReportDatatableClientSide(evaluationId);

        public async Task<List<PerformationEvaluationStudentTemplate>> GetStudentComplianceReportDatatableClientSide(Guid evaluationId)
            => await _repository.GetStudentComplianceReportDatatableClientSide(evaluationId);

        public async Task<ReturnedData<object>> GetEvaluatedUsers(SentParameters sentParameters, Guid evaluationId, string userId = null)
            => await _repository.GetEvaluatedUsers(sentParameters, evaluationId, userId);

        public async Task<ReturnedData<object>> GetSectionsReportDatatable(SentParameters sentParameters, Guid evaluationId, Guid academicDepartmentId, Guid? curriculumId,string searchValue)
            => await _repository.GetSectionsReportDatatable(sentParameters, evaluationId, academicDepartmentId, curriculumId,searchValue);

        public async Task<StatisticalReportCareerTeacherPerformanceTemplate> GetStatisticalReportCareerTeacherPerformance(Guid evaluationId, ClaimsPrincipal user)
            => await _repository.GetStatisticalReportCareerTeacherPerformance(evaluationId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentCompliancecDetailedReportDatatable(DataTablesStructs.SentParameters parameters ,Guid evaluationId, Guid? careerId, string searchValue)
            => await _repository.GetStudentCompliancecDetailedReportDatatable(parameters, evaluationId, careerId, searchValue);

        public async Task<PerformanceEvaluationUserDetail> GetResultBySection(Guid evaluationId, Guid sectionId, string toUserId)
            => await _repository.GetResultBySection(evaluationId, sectionId, toUserId);

        public async Task<List<ComplianceDetailedReport>> GetStudentCompliancecDetailedReport(Guid evaluationId, Guid? careerId, string serachValue)
            => await _repository.GetStudentCompliancecDetailedReport(evaluationId, careerId, serachValue);

        public async Task<List<PerformanceEvaluationCriteronTemplate>> GetTeacherReportPerformanceEvaluationCriterion(Guid? academicDepartmentId, Guid evaluationId)
            => await _repository.GetTeacherReportPerformanceEvaluationCriterion(academicDepartmentId, evaluationId);

        public async Task<StudentDebtResultTemplate> GenerateStudentDebts(Guid evaluationId)
            => await _repository.GenerateStudentDebts(evaluationId);

        public async Task<List<PerformanceEvaluationUserReportTemplate>> GetReportConsolidatedByAcademicDepartment(Guid evaluationId)
            => await _repository.GetReportConsolidatedByAcademicDepartment(evaluationId);

        public async Task<List<Question>> GetReportConsolidatedByQuestion(Guid evaluationId)
            => await _repository.GetReportConsolidatedByQuestion(evaluationId);
    }
}
