using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<bool> AnySupplierByName(string name)
        {
            return await _supplierRepository.AnySupplierByName(name);
        }

        public async Task<bool> AnySupplierByName(string name, string ruc)
        {
            return await _supplierRepository.AnySupplierByName(name, ruc);
        }

        public async Task<bool> AnySupplierByRUC(string ruc)
        {
            return await _supplierRepository.AnySupplierByRUC(ruc);
        }
        public async Task<bool> AnySupplierByNameDistint(string name, string ruc, Guid id)
            => await _supplierRepository.AnySupplierByNameDistint(name, ruc, id);

        public async Task<int> Count()
        {
            return await _supplierRepository.Count();
        }

        public async Task<Supplier> Get(Guid id)
        {
            return await _supplierRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _supplierRepository.GetSuppliersDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _supplierRepository.GetSuppliersDatatableByUser(sentParameters, userId, searchValue);
        }

        public async Task Delete(Supplier supplier) =>
            await _supplierRepository.Delete(supplier);

        public async Task Insert(Supplier supplier) =>
            await _supplierRepository.Insert(supplier);

        public async Task Update(Supplier supplier) =>
            await _supplierRepository.Update(supplier);
        public async Task<object> GetSuppliers()
            => await _supplierRepository.GetSuppliers();
    }
}
