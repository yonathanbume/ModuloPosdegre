using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionRequirement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionRequirementService
    {
        Task AddAsync(AdmissionRequirement admissionRequirement);
        Task UpdateAsync(AdmissionRequirement admissionRequirement);
        Task InsertAdmissionRequirement(AdmissionRequirement admissionRequirement);
        Task UpdateAdmissionRequirement(AdmissionRequirement admissionRequirement);
        Task DeleteAdmissionRequirement(AdmissionRequirement admissionRequirement);
        Task<AdmissionRequirement> GetAdmissionRequirementById(Guid id);
        Task<IEnumerable<AdmissionRequirement>> GetAllAdmissionRequirements();

        Task<object> GetPostulantRequiremtns(Guid admissionTypeId, Guid postulantId);
        Task<List<AdmissionRequirement>> GetAdmissionRequirementByAdmissionTypeId(Guid id);
        Task<object> GetRequirement(Guid id);
        Task DeleteAdmissionRequirementById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantRequirementsDatatable(Guid postulantId);
        Task<List<PostulantRequirementTemplate>> GetPostulantRequirements(Guid postulantId);
    }
}
