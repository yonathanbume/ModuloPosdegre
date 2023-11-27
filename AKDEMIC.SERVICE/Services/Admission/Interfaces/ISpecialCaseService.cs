using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.SpecialCase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface ISpecialCaseService
    {
        Task<bool> AnyByAdmissiontypeIdAndCareerId(Guid admissionTypeId, Guid careerId,Guid? specialCaseId=null);
        Task Insert(SpecialCase specialCase);
        Task<SpecialCase> Get(Guid id);
        Task<List<SpecialCaseTemplate>> GetSpecialCasesByAdmissionTypeId(Guid admissionTypeId);
        Task DeleteById(Guid id);
        Task Update(SpecialCase specialCase);
        Task<List<SpecialCase>> GetAllByAdmissionTypeId(Guid admissionTypeId);
    }
}
