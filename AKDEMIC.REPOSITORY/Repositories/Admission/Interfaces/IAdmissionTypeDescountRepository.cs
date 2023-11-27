using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionTypeDescountRepository : IRepository<AdmissionTypeDescount>
    {
        Task<List<AdmissionTypeDescount>> GetAdmissionTypeDescountByTermId(Guid termId);
        Task<AdmissionTypeDescount> GetAdmissionTypeDescountByAdmissionTypeIdAndTermId(Guid admissionTypeId, Guid id);
    }
}
