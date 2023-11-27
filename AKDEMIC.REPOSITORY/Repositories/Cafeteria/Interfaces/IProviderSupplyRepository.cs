using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IProviderSupplyRepository : IRepository<ProviderSupply>
    {
        Task<DataTablesStructs.ReturnedData<ProviderSupply>> GetProviderSupplyDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search);
        Task<bool> Exist(ProviderSupply providerSupply);
    }
}
