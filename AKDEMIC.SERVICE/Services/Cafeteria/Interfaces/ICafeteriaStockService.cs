using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ICafeteriaStockService
    {
        Task Insert(CafeteriaStock cafeteriaStock);
        Task<CafeteriaStock> Get(Guid id);
        Task Update(CafeteriaStock cafeteriaStock);
        Task DeleteById(Guid id);
        Task<decimal> GetStockByProviderSupplySum(Guid ProviderSupplyId, byte Turn);
        Task<object> GetSupplyByProvider(Guid ProviderId, byte TurnType);
        Task<DataTablesStructs.ReturnedData<object>> GetStock(DataTablesStructs.SentParameters sentParameters, Guid? providerId, byte turnType, string searchValue = null);
        Task<CafeteriaStock> GetStockByProviderSupplyIdAndTurn(Guid ProviderSupplyId, byte Turn);
    }
}
