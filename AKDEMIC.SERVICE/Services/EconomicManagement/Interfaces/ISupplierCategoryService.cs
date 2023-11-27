using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ISupplierCategoryService
    {
        Task<SupplierCategory> Get(Guid id);
        Task<IEnumerable<SupplierCategory>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetSupplierCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSuppliersReportDatatable(DataTablesStructs.SentParameters sentParameters,Guid? supplierCategoryId = null, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task Delete(SupplierCategory supplierCategory);
        Task Insert(SupplierCategory supplierCategory);
        Task Update(SupplierCategory supplierCategory);
    }
}
