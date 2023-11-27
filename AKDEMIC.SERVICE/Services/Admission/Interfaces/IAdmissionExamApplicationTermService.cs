using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamApplicationTermService
    {
        Task<object> GetApplicationTermsByAdmissionExamId(Guid admissionExamId);
        Task InsertRange(List<AdmissionExamApplicationTerm> applicationTerms);
        Task<bool> AnyByExamIdAndAppTermId(Guid examId, Guid applicationTermId);
        Task<bool> HasPrincipal(Guid applicationTermId, Guid? admissionExamId = null);
        Task DeleteApplicationTermsByAdmissionExamId(Guid admissionExamId);
        void RemoveRange(List<AdmissionExamApplicationTerm> applicationTerms);
        Task<List<AdmissionExamApplicationTerm>> GetApplicationTermsByExamId(Guid admissionExamId);
    }
}
