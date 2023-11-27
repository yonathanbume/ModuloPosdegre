using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.SpecialCase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface ISpecialCaseRepository : IRepository<SpecialCase>
    {
        Task<List<SpecialCaseTemplate>> GetSpecialCasesByAdmissionTypeId(Guid admissionTypeId);
        Task<bool> AnyByAdmissiontypeIdAndCareerId(Guid admissionTypeId, Guid careerId, Guid? specialCaseId = null);
        Task<List<SpecialCase>> GetAllByAdmissionTypeId(Guid admissionTypeId);
    }
}
