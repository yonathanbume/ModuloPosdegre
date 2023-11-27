using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class SupplierCategoryService : ISupplierCategoryService
    {
        private readonly ISupplierCategoryRepository _supplierCategoryRepository;

        public SupplierCategoryService(ISupplierCategoryRepository supplierCategoryRepository)
        {
            _supplierCategoryRepository = supplierCategoryRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _supplierCategoryRepository.AnyByName(name,id);

        public Task Delete(SupplierCategory supplierCategory)
            => _supplierCategoryRepository.Delete(supplierCategory);

        public Task<SupplierCategory> Get(Guid id)
            => _supplierCategoryRepository.Get(id);

        public Task<IEnumerable<SupplierCategory>> GetAll()
            => _supplierCategoryRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetSupplierCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _supplierCategoryRepository.GetSupplierCategoriesDatatable(sentParameters,searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetSuppliersReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? supplierCategoryId = null, string searchValue = null)
            => _supplierCategoryRepository.GetSuppliersReportDatatable(sentParameters, supplierCategoryId, searchValue);

        public Task Insert(SupplierCategory supplierCategory)
            => _supplierCategoryRepository.Insert(supplierCategory);

        public Task Update(SupplierCategory supplierCategory)
            => _supplierCategoryRepository.Update(supplierCategory);
    }
}
