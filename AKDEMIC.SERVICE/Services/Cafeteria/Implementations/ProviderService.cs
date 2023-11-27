using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _providerRepository.DeleteById(id);
        }

        public async Task<Provider> Get(Guid id)
        {
            return await _providerRepository.Get(id);
        }

        public async Task<Provider> GetProviderByUserId(string UserId)
        {
            return await _providerRepository.GetProviderByUserId(UserId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProvidersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _providerRepository.GetProvidersDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetSelectProviders()
        {
            return await _providerRepository.GetSelectProviders();
        }

        public async Task Insert(Provider provider)
        {
            await _providerRepository.Insert(provider);
        }

        public async Task Update(Provider provider)
        {
            await _providerRepository.Update(provider);
        }

        public async Task<bool> ValidateProviderCode(Guid? providerId, string code)
        {
            return await _providerRepository.ValidateProviderCode(providerId, code);
        }
    }
}
