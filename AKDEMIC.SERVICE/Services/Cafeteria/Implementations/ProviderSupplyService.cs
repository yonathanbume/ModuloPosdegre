using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class ProviderSupplyService : IProviderSupplyService
    {
        private IProviderSupplyRepository _providerRepository;
        public ProviderSupplyService(IProviderSupplyRepository providerSupplyRepository)
        {
            _providerRepository = providerSupplyRepository;
        }

        public async Task DeleteById(Guid providersupplyid)
        {
            await _providerRepository.DeleteById(providersupplyid);
        }

        public async Task<bool> Exists(ProviderSupply providerSupply)
        {
            return await _providerRepository.Exist(providerSupply);
        }

        public async Task<DataTablesStructs.ReturnedData<ProviderSupply>> GetProviderSupplyDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search)
        {
            return await _providerRepository.GetProviderSupplyDatatable(sentParameters, id, search);
        }

        public async Task Insert(ProviderSupply providerSupply)
        {
            await _providerRepository.Insert(providerSupply);
        }
    }
}
