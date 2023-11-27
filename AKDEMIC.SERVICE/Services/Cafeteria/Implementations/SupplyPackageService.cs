using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using AKDEMIC.SERVICE.Services.Cafeteria.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Implementations
{
    public class SupplyPackageService : ISupplyPackageService
    {
        private readonly ISupplyPackageRepository _supplyPackageRepository;
        public SupplyPackageService(ISupplyPackageRepository supplyPackageRepository)
        {
            _supplyPackageRepository = supplyPackageRepository;
        }

        public async Task Delete(SupplyPackage model)
        {
            await _supplyPackageRepository.Delete(model);
        }

        public async Task<SupplyPackage> Get(Guid Id)
        {
            return await _supplyPackageRepository.Get(Id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplyPackages(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _supplyPackageRepository.GetSupplyPackages(sentParameters, searchValue);
        }

        public async Task<object> GetSupplyPackageSelect()
        {
            return await _supplyPackageRepository.GetSupplyPackageSelect();
        }

        public async Task Insert(SupplyPackage model)
        {
            await _supplyPackageRepository.Insert(model);
        }

        public async Task Update(SupplyPackage model)
        {
            await _supplyPackageRepository.Update(model);
        }
    }
}
