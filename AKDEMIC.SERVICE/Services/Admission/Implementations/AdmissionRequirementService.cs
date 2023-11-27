using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionRequirement;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionRequirementService : IAdmissionRequirementService
    {
        private readonly IAdmissionRequirementRepository _admissionRequirementRepository;

        public AdmissionRequirementService(IAdmissionRequirementRepository admissionRequirementRepository)
        {
            _admissionRequirementRepository = admissionRequirementRepository;
        }

        public async Task AddAsync(AdmissionRequirement admissionRequirement)
            => await _admissionRequirementRepository.Add(admissionRequirement);
        public async Task UpdateAsync(AdmissionRequirement admissionRequirement)
            => await _admissionRequirementRepository.Update(admissionRequirement);
        public async Task InsertAdmissionRequirement(AdmissionRequirement admissionRequirement) =>
            await _admissionRequirementRepository.Insert(admissionRequirement);

        public async Task UpdateAdmissionRequirement(AdmissionRequirement admissionRequirement) =>
            await _admissionRequirementRepository.Update(admissionRequirement);

        public async Task DeleteAdmissionRequirement(AdmissionRequirement admissionRequirement) =>
            await _admissionRequirementRepository.Delete(admissionRequirement);

        public async Task<AdmissionRequirement> GetAdmissionRequirementById(Guid id) =>
            await _admissionRequirementRepository.Get(id);

        public async Task<IEnumerable<AdmissionRequirement>> GetAllAdmissionRequirements() =>
            await _admissionRequirementRepository.GetAll();

        public async Task<object> GetPostulantRequiremtns(Guid admissionTypeId, Guid postulantId)
            => await _admissionRequirementRepository.GetPostulantRequiremtns(admissionTypeId, postulantId);
        public async Task<List<AdmissionRequirement>> GetAdmissionRequirementByAdmissionTypeId(Guid id)
            => await _admissionRequirementRepository.GetAdmissionRequirementByAdmissionTypeId(id);
        public async Task<object> GetRequirement(Guid id)
            => await _admissionRequirementRepository.GetRequirement(id);

        public async Task DeleteAdmissionRequirementById(Guid id)
        {
            await _admissionRequirementRepository.DeleteById(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantRequirementsDatatable(Guid postulantId)
            => await _admissionRequirementRepository.GetPostulantRequirementsDatatable(postulantId);

        public async Task<List<PostulantRequirementTemplate>> GetPostulantRequirements(Guid postulantId)
            => await _admissionRequirementRepository.GetPostulantRequirements(postulantId);
    }
}
