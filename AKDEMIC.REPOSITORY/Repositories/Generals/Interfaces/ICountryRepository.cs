using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country> GetCountryByCode(string code);
        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetCountryJson(string q);
        Task<Country> GetByCode(string cell);
        Task<Guid> GetGuidByCode(string countryCode);
        Task<Guid> GetGuidFirst();
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllCountriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}