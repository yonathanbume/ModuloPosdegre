using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        public ExchangeRateService(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task DeleteById(Guid id)
            => await _exchangeRateRepository.DeleteById(id);

        public async Task<ExchangeRate> Get(Guid id)
            => await _exchangeRateRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _exchangeRateRepository.GetDatatable(sentParameters);

        public async Task Insert(ExchangeRate exchangeRate)
            => await _exchangeRateRepository.Insert(exchangeRate);

        public async Task Update(ExchangeRate exchangeRate)
            => await _exchangeRateRepository.Update(exchangeRate);
    }
}
