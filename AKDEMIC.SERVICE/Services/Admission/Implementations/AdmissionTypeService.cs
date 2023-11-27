using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionTypeService : IAdmissionTypeService
    {
        private readonly IAdmissionTypeRepository _admissionTypeRepository;

        public AdmissionTypeService(IAdmissionTypeRepository admissionTypeRepository)
        {
            _admissionTypeRepository = admissionTypeRepository;
        }

        public async Task InsertAdmissionType(AdmissionType admissionType) =>
           await _admissionTypeRepository.Insert(admissionType);

        public async Task UpdateAdmissionType(AdmissionType admissionType) =>
            await _admissionTypeRepository.Update(admissionType);

        public async Task DeleteAdmissionType(AdmissionType admissionType) =>
            await _admissionTypeRepository.Delete(admissionType);

        public async Task<AdmissionType> GetAdmissionTypeById(Guid id) =>
            await _admissionTypeRepository.Get(id);

        public async Task<IEnumerable<AdmissionType>> GetAllAdmissionTypes() =>
            await _admissionTypeRepository.GetAll();
        public async Task<IEnumerable<Select2Structs.Result>> GetAdmissionTypesSelect2ClientSide(bool includeTitle = false)
            => await _admissionTypeRepository.GetAdmissionTypesSelect2ClientSide(includeTitle);

        public async Task<AdmissionTypeDescount> GetAdmissionTypeDescounts(Guid admissionTypeId, Guid currentApplicationTermId)
            => await _admissionTypeRepository.GetAdmissionTypeDescounts(admissionTypeId, currentApplicationTermId);

        public async Task<List<Select2Structs.Result>> GetAdmissionTypeJson()
            => await _admissionTypeRepository.GetAdmissionTypeJson();
        public async Task<AdmissionType> GetNameByCellExcel(string cell)
            => await _admissionTypeRepository.GetNameByCellExcel(cell);

        public async Task<object> GetAmissionTypeIrregular()
            => await _admissionTypeRepository.GetAmissionTypeIrregular();

        public async Task<Guid> GetGuidFirst()
            => await _admissionTypeRepository.GetGuidFirst();

        public async Task<object> GetAdmissionTypesTerm(Guid termId)
            => await _admissionTypeRepository.GetAdmissionTypesTerm(termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmissionTypesCategories(DataTablesStructs.SentParameters sentParameters, string search, bool showInactive)
            => await _admissionTypeRepository.GetAdmissionTypesCategories(sentParameters, search, showInactive);

        public async Task AddAsync(AdmissionType admissionType)
            => await _admissionTypeRepository.Add(admissionType);

        public async Task DeleteAdmissionTypeById(Guid id)
        {
            await _admissionTypeRepository.DeleteById(id);
        }

        public async Task<List<AdmissionType>> GetByAbbreviation(string v)
        {
            return await _admissionTypeRepository.GetByAbbreviation(v);
        }

        public async Task<AdmissionType> GetAgreementAdmissionType()
            => await _admissionTypeRepository.GetAgreementAdmissionType();
    }
}
