using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionTypeDescountService : IAdmissionTypeDescountService
    {
        private readonly IAdmissionTypeDescountRepository _admissionTypeDescountRepository;

        public AdmissionTypeDescountService(IAdmissionTypeDescountRepository admissionTypeDescountRepository)
        {
            _admissionTypeDescountRepository = admissionTypeDescountRepository;
        }

        public async Task InsertAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount) =>
            await _admissionTypeDescountRepository.Insert(admissionTypeDescount);

        public async Task UpdateAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount) =>
            await _admissionTypeDescountRepository.Update(admissionTypeDescount);

        public async Task DeleteAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount) =>
            await _admissionTypeDescountRepository.Delete(admissionTypeDescount);

        public void RemoveRange(IEnumerable<AdmissionTypeDescount> admissionTypeDescounts)
            => _admissionTypeDescountRepository.RemoveRange(admissionTypeDescounts);

        public async Task AddAsync(AdmissionTypeDescount admissionTypeDescount)
            => await _admissionTypeDescountRepository.Add(admissionTypeDescount);

        public async Task AddRange(List<AdmissionTypeDescount> admissionTypeDescounts)
            => await _admissionTypeDescountRepository.AddRange(admissionTypeDescounts);
        public async Task<AdmissionTypeDescount> GetAdmissionTypeDescountById(Guid id) =>
            await _admissionTypeDescountRepository.Get(id);

        public async Task<IEnumerable<AdmissionTypeDescount>> GetAllAdmissionTypeDescounts() =>
            await _admissionTypeDescountRepository.GetAll();

        public async Task<List<AdmissionTypeDescount>> GetAdmissionTypeDescountByTermId(Guid termId)
            => await _admissionTypeDescountRepository.GetAdmissionTypeDescountByTermId(termId);
        public async Task<AdmissionTypeDescount> GetAdmissionTypeDescountByAdmissionTypeIdAndTermId(Guid admissionTypeId, Guid id)
            => await _admissionTypeDescountRepository.GetAdmissionTypeDescountByAdmissionTypeIdAndTermId(admissionTypeId, id);

        public async Task InsertRange(IEnumerable<AdmissionTypeDescount> admissionTypeDescount)
        {
            await _admissionTypeDescountRepository.InsertRange(admissionTypeDescount);
        }
    }
}
