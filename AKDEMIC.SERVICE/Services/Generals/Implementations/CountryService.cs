using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> GetAll()
            => await _countryRepository.GetAll();
        Task<Country> ICountryService.Get(Guid countryId)
            => _countryRepository.Get(countryId);

        Task<object> ICountryService.GetAllAsSelect2ClientSide()
            => _countryRepository.GetAllAsSelect2ClientSide();

        Task<Country> ICountryService.GetCountryByCode(string code)
            => _countryRepository.GetCountryByCode(code);

        public async Task<object> GetCountryJson(string q)
            => await _countryRepository.GetCountryJson(q);

        public async Task<Country> GetByCode(string cell)
            => await _countryRepository.GetByCode(cell);

        public async Task<Guid> GetGuidByCode(string countryCode)
            => await _countryRepository.GetGuidByCode(countryCode);

        public async Task<Guid> GetGuidFirst()
            => await _countryRepository.GetGuidFirst();

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _countryRepository.AnyByCode(code, id);

        public Task Insert(Country country)
            => _countryRepository.Insert(country);

        public Task Update(Country country)
            => _countryRepository.Update(country);

        public Task Delete(Country country)
            => _countryRepository.Delete(country);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllCountriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _countryRepository.GetAllCountriesDatatable(sentParameters, searchValue);
    }
}