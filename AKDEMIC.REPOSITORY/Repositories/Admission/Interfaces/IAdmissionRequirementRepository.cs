using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionRequirement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionRequirementRepository : IRepository<AdmissionRequirement>
    {
        Task<object> GetPostulantRequiremtns(Guid admissionTypeId, Guid postulantId);
        Task Update(AdmissionRequirement admissionRequirement);
        Task<List<AdmissionRequirement>> GetAdmissionRequirementByAdmissionTypeId(Guid id);
        Task<object> GetRequirement(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantRequirementsDatatable(Guid postulantId);
        Task<List<PostulantRequirementTemplate>> GetPostulantRequirements(Guid postulantId);
    }
}
