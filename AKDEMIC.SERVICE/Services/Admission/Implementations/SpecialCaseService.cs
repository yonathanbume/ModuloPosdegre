using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.SpecialCase;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class SpecialCaseService : ISpecialCaseService
    {
        private readonly ISpecialCaseRepository _specialCaseRepository;
        public SpecialCaseService(ISpecialCaseRepository specialCaseRepository)
        {
            _specialCaseRepository=specialCaseRepository;
        }

        public async Task<bool> AnyByAdmissiontypeIdAndCareerId(Guid admissionTypeId, Guid careerId,Guid? specialCaseId=null)
        {
            return await _specialCaseRepository.AnyByAdmissiontypeIdAndCareerId(admissionTypeId, careerId, specialCaseId);
        }

        public async Task DeleteById(Guid id)
        {
            await _specialCaseRepository.DeleteById(id);
        }

        public async Task<SpecialCase> Get(Guid id)
        {
            return await _specialCaseRepository.Get(id);
        }

        public async Task<List<SpecialCase>> GetAllByAdmissionTypeId(Guid admissionTypeId)
            => await _specialCaseRepository.GetAllByAdmissionTypeId(admissionTypeId);

        public async Task<List<SpecialCaseTemplate>> GetSpecialCasesByAdmissionTypeId(Guid admissionTypeId)
        {
            return await _specialCaseRepository.GetSpecialCasesByAdmissionTypeId(admissionTypeId);
        }

        public async Task Insert(SpecialCase specialCase)
        {
            await _specialCaseRepository.Insert(specialCase);
        }

        public async Task Update(SpecialCase specialCase)
        {
            await _specialCaseRepository.Update(specialCase);
        }
    }
}
