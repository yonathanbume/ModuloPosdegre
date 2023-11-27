using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IEvaluationReportService
    {
        Task InsertEvaluationReport(EvaluationReport evaluationReport);
        Task UpdateEvaluationReport(EvaluationReport evaluationReport);
        Task DeleteEvaluationReport(EvaluationReport evaluationReport);
        Task<EvaluationReport> GetEvaluationReportById(Guid id);
        Task<IEnumerable<EvaluationReport>> GetaLLEvaluationReports();
        Task<DataTablesStructs.ReturnedData<object>> GetAllEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid school, Guid career, string searchValue,ClaimsPrincipal user = null,byte? status = null, Guid? termId = null, byte? type = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSearchEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters,int? resolutionNumber = null, Guid? termId = null, Guid? careerId = null, Guid? curriculumId = null, Guid? courseId = null,string  code = null, string courseSearch = null, bool? onlyReceived = null);
        Task<EvaluationReport> GetEvaluationReportBySectionId(Guid sectionId);
        Task<EvaluationReport> GetEvalutionReportByTeacherIdAndCourseId(Guid courseId, string teacherId);
        Task<EvaluationReport> GetEvaluationReportByFilters(Guid? sectionId, Guid? courseId, Guid? termId, byte type);
        Task<int> GetNumberByFilter(byte type);
        Task<IEnumerable<EvaluationReportExcelTemplate>> GetEvaluationReportExcel(Guid termId, byte? status);
        Task SaveChanges();
        Task<EvaluationReport> Get(Guid id);
        Task<List<EvaluationReport>> GetEvaluationReportsByCode(string code, Guid termId);
        Task<int> GetMaxNumber(Guid termId);
        Task<EvaluationReportInformationTemplate> GetEvaluationReportInformation(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null, bool isRegister= false);
        Task<EvaluationReportInformationTemplate> GetEvaluationReportDeferredExamInformation(Guid deferredExamId);
        Task<EvaluationReportInformationTemplate> GetEvaluationReportExtraordinaryEvaluationInformation(Guid extraordinaryEvaluationId);
        Task<EvaluationReportInformationTemplate> GetEvaluationReportCorrectionExamInformation(Guid correctionExamId);
    }
}
