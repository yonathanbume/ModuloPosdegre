using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictService(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<IEnumerable<District>> GetAll()
            => await _districtRepository.GetAll();
        public async Task<District> Get(Guid districtId)
        {
            return await _districtRepository.Get(districtId);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetDistrictsSelect2ClientSide(Guid provinceId)
        {
            return await _districtRepository.GetDistrictsSelect2ClientSide(provinceId);
        }

        public async Task<object> GetDistrictsJson(Guid pid, string q)
            => await _districtRepository.GetDistrictsJson(pid, q);

        public async Task<District> GetByNameAndProvinceId(Guid provinceId, string cell)
            => await _districtRepository.GetByNameAndProvinceId(provinceId, cell);

        public async Task<Guid> GetGuidByName(string name)
            => await _districtRepository.GetGuidByName(name);
        public async Task<object> GetDistricts()
            => await _districtRepository.GetDistricts();

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _districtRepository.AnyByCode(code, id);

        public Task Insert(District district)
            => _districtRepository.Insert(district);

        public Task Update(District district)
            => _districtRepository.Update(district);

        public Task Delete(District district)
            => _districtRepository.Delete(district);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDistrictDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null, Guid? provinceId = null)
            => _districtRepository.GetAllDistrictDatatable(sentParameters, searchValue, countryId, departmentId, provinceId);
    }
}
