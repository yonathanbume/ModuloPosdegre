using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class CafeteriaStockService : ICafeteriaStockService
    {
        private readonly ICafeteriaStockRepository _cafeteriaStockRepository;

        public CafeteriaStockService(ICafeteriaStockRepository cafeteriaStockRepository)
        {
            _cafeteriaStockRepository = cafeteriaStockRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _cafeteriaStockRepository.DeleteById(id);
        }

        public async Task<CafeteriaStock> Get(Guid id)
        {
            return await _cafeteriaStockRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStock(DataTablesStructs.SentParameters sentParameters, Guid? providerId, byte turnType,  string searchValue = null)
        {
            return await _cafeteriaStockRepository.GetStock(sentParameters, providerId, turnType, searchValue);
        }

        public async Task<CafeteriaStock> GetStockByProviderSupplyIdAndTurn(Guid ProviderSupplyId, byte Turn)
        {
            return await _cafeteriaStockRepository.GetStockByProviderSupplyIdAndTurn(ProviderSupplyId, Turn);
        }

        public async Task<decimal> GetStockByProviderSupplySum(Guid ProviderSupplyId, byte Turn)
        {
            return await _cafeteriaStockRepository.GetStockByProviderSupplySum(ProviderSupplyId, Turn);
        }

        public async Task<object> GetSupplyByProvider(Guid ProviderId, byte TurnType)
        {
            return await _cafeteriaStockRepository.GetSupplyByProvider(ProviderId, TurnType);
        }

        public async Task Insert(CafeteriaStock cafeteriaStock)
        {
            await _cafeteriaStockRepository.Insert(cafeteriaStock);
        }

        public async Task Update(CafeteriaStock cafeteriaStock)
        {
            await _cafeteriaStockRepository.Update(cafeteriaStock);
        }
    }
}
