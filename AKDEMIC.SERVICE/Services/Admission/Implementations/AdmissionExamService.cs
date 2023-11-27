using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamService : IAdmissionExamService
    {
        private readonly IAdmissionExamRepository _admissionExamRepository;

        public AdmissionExamService(IAdmissionExamRepository admissionExamRepository)
        {
            _admissionExamRepository = admissionExamRepository;
        }

        public async Task Insert(AdmissionExam admissionExam) =>
            await _admissionExamRepository.Insert(admissionExam);

        public async Task Update(AdmissionExam admissionExam) =>
            await _admissionExamRepository.Update(admissionExam);

        public async Task DeleteExamById(Guid id) =>
            await _admissionExamRepository.DeleteExamById(id);

        public async Task<AdmissionExam> Get(Guid id) =>
            await _admissionExamRepository.Get(id);

        public async Task<IEnumerable<AdmissionExam>> GetAll() =>
            await _admissionExamRepository.GetAll();
        public async Task<object> GetAmissionExam()
            => await _admissionExamRepository.GetAmissionExam();
        public async Task<AdmissionExam> GetWithData(Guid id)
            => await _admissionExamRepository.GetWithData(id);
        public async Task<List<AdmissionExam>> GetClassroomManagement(Guid id)
            => await _admissionExamRepository.GetClassroomManagement(id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _admissionExamRepository.GetPostulantDatatable(sentParameters, id);

        public async Task<object> GetAdmissionExams()
        {
            return await _admissionExamRepository.GetAdmissionExams();
        }

        public async Task<List<AdmissionExam>> GetByApplicationTermId(Guid applicationTermId)
        {
            return await _admissionExamRepository.GetByApplicationTermId(applicationTermId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmissionExamDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null)
            => await _admissionExamRepository.GetAdmissionExamDatatable(sentParameters, applicationTermId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantAssistanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid examId, byte status = 1)
            => await _admissionExamRepository.GetPostulantAssistanceReportDatatable(sentParameters, examId, status);

        public async Task<List<AdmissionExam>> GetActiveApplicationTermsExams()
            => await _admissionExamRepository.GetActiveApplicationTermsExams();
    }
}
