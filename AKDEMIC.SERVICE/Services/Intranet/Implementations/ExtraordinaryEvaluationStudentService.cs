using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtraordinaryEvaluationStudentService : IExtraordinaryEvaluationStudentService
    {
        private readonly IExtraordinaryEvaluationStudentRepository _extraordinaryEvaluationStudentRepository;
        public ExtraordinaryEvaluationStudentService(IExtraordinaryEvaluationStudentRepository extraordinaryEvaluationStudentRepository)
        {
            _extraordinaryEvaluationStudentRepository = extraordinaryEvaluationStudentRepository;
        }

        public async Task Delete(ExtraordinaryEvaluationStudent entity)
            => await _extraordinaryEvaluationStudentRepository.Delete(entity);

        public async Task<ExtraordinaryEvaluationStudent> Get(Guid id)
            => await _extraordinaryEvaluationStudentRepository.Get(id);

        public async Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationId(Guid extraordinaryEvaluationId)
            => await _extraordinaryEvaluationStudentRepository.GetByExtraordinaryEvaluationId(extraordinaryEvaluationId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid extraordinaryEvaluationId, string searchValue)
            => await _extraordinaryEvaluationStudentRepository.GetStudentsDatatable(parameters, extraordinaryEvaluationId, searchValue);

        public async Task<object> GetStudentsDatatableClientSide(Guid extraordinaryEvaluationId)
            => await _extraordinaryEvaluationStudentRepository.GetStudentsDatatableClientSide(extraordinaryEvaluationId);

        public async Task Insert(ExtraordinaryEvaluationStudent entity)
            => await _extraordinaryEvaluationStudentRepository.Insert(entity);

        //public async Task DeleteById(Guid id) => await _extraordinaryEvaluationStudentRepository.DeleteById(id);

        //public async Task<ExtraordinaryEvaluationStudent> Get(Guid id) => await _extraordinaryEvaluationStudentRepository.Get(id);

        //public async Task Insert(ExtraordinaryEvaluationStudent extraordinaryEvaluationStudent) => await _extraordinaryEvaluationStudentRepository.Insert(extraordinaryEvaluationStudent);

        public async Task Update(ExtraordinaryEvaluationStudent extraordinaryEvaluationStudent) => await _extraordinaryEvaluationStudentRepository.Update(extraordinaryEvaluationStudent);

        public async Task<object> GetEnrollmentDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
            => await _extraordinaryEvaluationStudentRepository.GetEnrollmentDataDatatable(sentParameters, studentId, termId);

        public async Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationIdWithData(Guid extraordinaryEvaluationId)
            => await _extraordinaryEvaluationStudentRepository.GetByExtraordinaryEvaluationIdWithData(extraordinaryEvaluationId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentCurrentEvaluationsClientSideDatatable(Guid studentId, string searchValue)
            => await _extraordinaryEvaluationStudentRepository.GetStudentCurrentEvaluationsClientSideDatatable(studentId, searchValue);

        public Task<bool> IsPendingFromQualify(Guid extraordinaryEvaluationId)
            => _extraordinaryEvaluationStudentRepository.IsPendingFromQualify(extraordinaryEvaluationId);

        public Task<ExtraordinaryEvaluationReportTemplate> GetEvaluationReportInformation(Guid extraordinaryEvaluationId)
            => _extraordinaryEvaluationStudentRepository.GetEvaluationReportInformation(extraordinaryEvaluationId);

        //public async Task<ExtraordinaryEvaluationStudent> GetWithData(Guid id)
        //    => await _extraordinaryEvaluationStudentRepository.GetWithData(id);

        //public async Task<object> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string teacherId = null, Guid? courseId = null, string search = null)
        //    => await _extraordinaryEvaluationStudentRepository.GetStudentsDataDatatable(sentParameters, termId, teacherId, courseId, search);

    }
}
