using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ICafeteriaStockRepository : IRepository<CafeteriaStock>
    {
        Task<decimal> GetStockByProviderSupplySum(Guid ProviderSupplyId, byte Turn);       
        Task<DataTablesStructs.ReturnedData<object>> GetStock(DataTablesStructs.SentParameters sentParameters, Guid? providerId, byte turnType, string searchValue = null);
        Task<CafeteriaStock> GetStockBySupplyIdAndProviderId(Guid SupplyId, Guid ProviderId);
        Task<object> GetSupplyByProvider(Guid ProviderId, byte TurnType);
        Task<CafeteriaStock> GetStockByProviderSupplyIdAndTurn(Guid ProviderSupplyId, byte Turn);

    }
}
