using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class EvaluationReportService : IEvaluationReportService
    {
        private readonly IEvaluationReportRepository _evaluationReportRepository;

        public EvaluationReportService(IEvaluationReportRepository evaluationReportRepository)
        {
            _evaluationReportRepository = evaluationReportRepository;
        }

        public async Task InsertEvaluationReport(EvaluationReport evaluationReport) =>
            await _evaluationReportRepository.Insert(evaluationReport);

        public async Task UpdateEvaluationReport(EvaluationReport evaluationReport) =>
            await _evaluationReportRepository.Update(evaluationReport);

        public async Task DeleteEvaluationReport(EvaluationReport evaluationReport) =>
            await _evaluationReportRepository.Delete(evaluationReport);

        public async Task<EvaluationReport> GetEvaluationReportById(Guid id) =>
            await _evaluationReportRepository.Get(id);

        public async Task<IEnumerable<EvaluationReport>> GetaLLEvaluationReports() =>
            await _evaluationReportRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid school, Guid career, string searchValue,ClaimsPrincipal user = null,byte? status = null, Guid? termId = null, byte? type = null)
            => await _evaluationReportRepository.GetAllEvaluationReportDatatable(sentParameters, faculty, school, career, searchValue,user,status,termId, type);

        public async Task<EvaluationReport> GetEvaluationReportBySectionId(Guid sectionId)
            => await _evaluationReportRepository.GetEvaluationReportBySectionId(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSearchEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters,int? resolutionNumber = null, Guid? termId = null, Guid? careerId = null, Guid? curriculumId = null, Guid? courseId = null,string code = null, string courseSearch = null, bool? onlyReceived = null)
        {
            return await _evaluationReportRepository.GetSearchEvaluationReportDatatable(sentParameters,resolutionNumber, termId, careerId, curriculumId, courseId,code, courseSearch, onlyReceived);
        }

        public async Task<EvaluationReport> GetEvalutionReportByTeacherIdAndCourseId(Guid courseId, string teacherId)
            => await _evaluationReportRepository.GetEvalutionReportByTeacherIdAndCourseId(courseId, teacherId);

        public async Task<IEnumerable<EvaluationReportExcelTemplate>> GetEvaluationReportExcel(Guid termId, byte? status)
            => await _evaluationReportRepository.GetEvaluationReportExcel(termId, status);

        public async Task SaveChanges()
        {
            await _evaluationReportRepository.SaveChanges();
        }

        public async Task<EvaluationReport> GetEvaluationReportByFilters(Guid? sectionId, Guid? courseId, Guid? termId, byte type)
            => await _evaluationReportRepository.GetEvaluationReportByFilters(sectionId, courseId, termId, type);

        public async Task<int> GetNumberByFilter(byte type)
            => await _evaluationReportRepository.GetNumberByFilter(type);

        public async Task<EvaluationReport> Get(Guid id)
            => await _evaluationReportRepository.Get(id);

        public async Task<List<EvaluationReport>> GetEvaluationReportsByCode(string code, Guid termId)
            => await _evaluationReportRepository.GetEvaluationReportsByCode(code, termId);

        public async Task<int> GetMaxNumber(Guid termId)
            => await _evaluationReportRepository.GetMaxNumber(termId);

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportInformation(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null, bool isRegister = false)
            => await _evaluationReportRepository.GetEvaluationReportInformation(sectionId, code, issueDate, receptionDate, isRegister);

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportDeferredExamInformation(Guid deferredExamId)
            => await _evaluationReportRepository.GetEvaluationReportDeferredExamInformation(deferredExamId);

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportExtraordinaryEvaluationInformation(Guid extraordinaryEvaluationId)
            => await _evaluationReportRepository.GetEvaluationReportExtraordinaryEvaluationInformation(extraordinaryEvaluationId);

        public async Task<EvaluationReportInformationTemplate> GetEvaluationReportCorrectionExamInformation(Guid correctionExamId)
            => await _evaluationReportRepository.GetEvaluationReportCorrectionExamInformation(correctionExamId);
    }
}
