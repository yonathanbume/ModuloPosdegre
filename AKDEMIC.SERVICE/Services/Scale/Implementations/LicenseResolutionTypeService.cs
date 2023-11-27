using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class LicenseResolutionTypeService: ILicenseResolutionTypeService
    {
        private readonly ILicenseResolutionTypeRepository _licenseResolutionTypeRepository;

        public LicenseResolutionTypeService(ILicenseResolutionTypeRepository licenseResolutionTypeRepository)
        {
            _licenseResolutionTypeRepository = licenseResolutionTypeRepository;
        }

        public async Task Delete(LicenseResolutionType licenseResolutionType)
            => await _licenseResolutionTypeRepository.Delete(licenseResolutionType);

        public async Task<LicenseResolutionType> Get(Guid id)
            => await _licenseResolutionTypeRepository.Get(id);

        public async Task<IEnumerable<LicenseResolutionType>> GetAll()
            => await _licenseResolutionTypeRepository.GetAll();

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _licenseResolutionTypeRepository.AnyByName(name,id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllLicenseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _licenseResolutionTypeRepository.GetAllLicenseDatatable(sentParameters, searchValue);

        public async Task Insert(LicenseResolutionType licenseResolutionType)
            => await _licenseResolutionTypeRepository.Insert(licenseResolutionType);

        public async Task Update(LicenseResolutionType licenseResolutionType)
            => await _licenseResolutionTypeRepository.Update(licenseResolutionType);
    }
}
