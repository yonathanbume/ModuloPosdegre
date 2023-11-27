using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IProviderRepository:IRepository<Provider>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetProvidersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSelectProviders();
        Task<Provider> GetProviderByUserId(string UserId);
        Task<bool> ValidateProviderCode(Guid? providerId, string code);
    }
}
