using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IProviderSupplyService
    {
        Task<DataTablesStructs.ReturnedData<ProviderSupply>> GetProviderSupplyDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search);
        Task DeleteById(Guid providersupplyid);
        Task Insert(ProviderSupply providerSupply);
        Task<bool> Exists(ProviderSupply providerSupply);
    }
}
