using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamApplicationTermRepository:IRepository<AdmissionExamApplicationTerm>
    {
        Task<object> GetApplicationTermsByAdmissionExamId(Guid admissionExamId);
        Task<bool> AnyByExamIdAndAppTermId(Guid examId, Guid applicationTermId);
        Task<bool> HasPrincipal(Guid applicationTermId, Guid? admissionExamId = null);
        Task DeleteApplicationTermsByAdmissionExamId(Guid admissionExamId);
        Task<List<AdmissionExamApplicationTerm>> GetApplicationTermsByExamId(Guid admissionExamId);
    }
}
