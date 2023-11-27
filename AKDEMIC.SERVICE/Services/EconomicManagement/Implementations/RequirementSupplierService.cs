using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class RequirementSupplierService : IRequirementSupplierService
    {
        private readonly IRequirementSupplierRepository _requirementSupplierRepository;

        public RequirementSupplierService(IRequirementSupplierRepository RequirementSupplierRepository)
        {
            _requirementSupplierRepository = RequirementSupplierRepository;
        }

        public async Task<int> Count()
        {
            return await _requirementSupplierRepository.Count();
        }

        public async Task<RequirementSupplier> Get(Guid id)
        {
            return await _requirementSupplierRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<RequirementSupplier>> GetRequirementSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _requirementSupplierRepository.GetRequirementSuppliersDatatable(sentParameters, searchValue);
        }

        public async Task Delete(RequirementSupplier RequirementSupplier) =>
            await _requirementSupplierRepository.Delete(RequirementSupplier);

        public async Task Insert(RequirementSupplier RequirementSupplier) =>
            await _requirementSupplierRepository.Insert(RequirementSupplier);

        public async Task Update(RequirementSupplier RequirementSupplier) =>
            await _requirementSupplierRepository.Update(RequirementSupplier);
        public async Task AddAsync(RequirementSupplier requirementSupplier)
            => await _requirementSupplierRepository.Add(requirementSupplier);
        public IQueryable<RequirementSupplier> GetQueryWithData(Guid id)
            =>  _requirementSupplierRepository.GetQueryWithData(id);
        public async Task<object> GetSelectSupplier(Guid id)
            => await _requirementSupplierRepository.GetSelectSupplier(id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetSupplierDatatable(DataTablesStructs.SentParameters sentParameters, Guid urquid, string search)
            => await _requirementSupplierRepository.GetSupplierDatatable(sentParameters, urquid, search);
        public async Task<bool> AnySupplierId(Guid supplierId, Guid requerimentId)
            => await _requirementSupplierRepository.AnySupplierId(supplierId, requerimentId);
    }
}
