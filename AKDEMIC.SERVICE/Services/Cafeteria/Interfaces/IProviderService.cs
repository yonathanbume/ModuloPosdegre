using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IProviderService
    {
        Task Insert(Provider provider);
        Task<Provider> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetProvidersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteById(Guid id);
        Task Update(Provider provider);
        Task<object> GetSelectProviders();
        Task<Provider> GetProviderByUserId(string UserId);
        Task<bool> ValidateProviderCode(Guid? providerId, string code);
    }
}
