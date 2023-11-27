using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class SupplyService : ISupplyService
    {
        private readonly ISupplyRepository _supplyRepository;
        public SupplyService(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _supplyRepository.DeleteById(id);
        }

        public async Task<Supply> Get(Guid id)
        {
            return await _supplyRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<Supply>> GetSuppliesDataTable(DataTablesStructs.SentParameters parameters, string search)
        {
            return await _supplyRepository.GetSuppliesDataTable(parameters, search);
        }

        public async Task<object> GetSupplySelect(Guid providerId)
        {
            return await _supplyRepository.GetSupplySelect(providerId);
        }

        public async Task<Select2Structs.ResponseParameters> GetSuppliesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _supplyRepository.GetSuppliesSelect2(requestParameters, searchValue);
        }

        public async Task Insert(Supply newSupply)
        {
            await _supplyRepository.Insert(newSupply);
        }

        public async Task Update(Supply editing)
        {
            await _supplyRepository.Update(editing);
        }
    }
}
