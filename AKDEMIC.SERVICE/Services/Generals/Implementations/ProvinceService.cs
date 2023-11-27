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
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceService(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task<IEnumerable<Province>> GetAll()
            => await _provinceRepository.GetAll();
        public async Task<Province> Get(Guid provinceId)
        {
            return await _provinceRepository.Get(provinceId);
        }

        public async Task<Tuple<int, List<Tuple<string, string, int>>>> GetProvincesQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _provinceRepository.GetProvincesQuantityReportByPaginationParameters(paginationParameter);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetProvincesSelect2ClientSide(Guid deparmentId)
        {
            return await _provinceRepository.GetProvincesSelect2ClientSide(deparmentId);
        }

        public async Task<List<Tuple<string, string, int>>> GetProvincesTypeQuantityReport(string search)
        {
            return await _provinceRepository.GetProvincesTypeQuantityReport(search);
        }

        public async Task<object> GetProvincesJson(Guid did, string q)
            => await _provinceRepository.GetProvincesJson(did, q);

        public async Task<Province> GetByNameAndDepartmentId(Guid departmentId, string cell)
            => await _provinceRepository.GetByNameAndDepartmentId(departmentId ,cell);

        public async Task<Guid> GetGuidByName(string name)
            => await _provinceRepository.GetGuidByName(name);
        public async Task<object> GetProvinces()
            => await _provinceRepository.GetProvinces();

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _provinceRepository.AnyByCode(code, id);

        public Task Insert(Province province)
            => _provinceRepository.Insert(province);

        public Task Update(Province province)
            => _provinceRepository.Update(province);

        public Task Delete(Province province)
            => _provinceRepository.Delete(province);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllProvinceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? countryId = null, Guid? departmentId = null)
            => _provinceRepository.GetAllProvinceDatatable(sentParameters, searchValue, countryId, departmentId);

        public Task<object> GetAllAsSelect2ClientSide(Guid? departmentId = null)
            => _provinceRepository.GetAllAsSelect2ClientSide(departmentId);
    }
}
