using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamApplicationTermService : IAdmissionExamApplicationTermService
    {
        private readonly IAdmissionExamApplicationTermRepository _admissionExamApplicationTermRepository;
        public AdmissionExamApplicationTermService(IAdmissionExamApplicationTermRepository admissionExamApplicationTermRepository)
        {
            _admissionExamApplicationTermRepository=admissionExamApplicationTermRepository;
        }

        public async Task<bool> AnyByExamIdAndAppTermId(Guid examId, Guid applicationTermId)
        {
            return await _admissionExamApplicationTermRepository.AnyByExamIdAndAppTermId(examId, applicationTermId);
        }

        public async Task DeleteApplicationTermsByAdmissionExamId(Guid admissionExamId)
        {
            await _admissionExamApplicationTermRepository.DeleteApplicationTermsByAdmissionExamId(admissionExamId);
        }

        public async Task<object> GetApplicationTermsByAdmissionExamId(Guid admissionExamId)
        {
            return await _admissionExamApplicationTermRepository.GetApplicationTermsByAdmissionExamId(admissionExamId);
        }

        public async Task<List<AdmissionExamApplicationTerm>> GetApplicationTermsByExamId(Guid admissionExamId)
            => await _admissionExamApplicationTermRepository.GetApplicationTermsByExamId(admissionExamId);


        public async Task<bool> HasPrincipal(Guid applicationTermId, Guid? admissionExamId = null)
        {
            return await _admissionExamApplicationTermRepository.HasPrincipal(applicationTermId, admissionExamId);
        }

        public async Task InsertRange(List<AdmissionExamApplicationTerm> applicationTerms) => await _admissionExamApplicationTermRepository.InsertRange(applicationTerms);

        public void RemoveRange(List<AdmissionExamApplicationTerm> applicationTerms)
            => _admissionExamApplicationTermRepository.RemoveRange(applicationTerms);
    }
}
