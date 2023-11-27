using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAll();
        Task<Country> Get(Guid countryId);
        Task<Country> GetCountryByCode(string code);

        Task<bool> AnyByCode(string code, Guid? id = null);
        Task Insert(Country country);
        Task Update(Country country);
        Task Delete(Country country);
        Task<DataTablesStructs.ReturnedData<object>> GetAllCountriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetCountryJson(string q);
        Task<Country> GetByCode(string cell);
        Task<Guid> GetGuidByCode(string countryCode);
        Task<Guid> GetGuidFirst();
    }
}