using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ISupplierService
    {
        Task<bool> AnySupplierByName(string name);
        Task<bool> AnySupplierByName(string name, string ruc);
        Task<bool> AnySupplierByRUC(string ruc);
        Task<bool> AnySupplierByNameDistint(string name, string ruc, Guid id);
        Task<int> Count();
        Task<Supplier> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task Delete(Supplier supplier);
        Task Insert(Supplier supplier);
        Task Update(Supplier supplier);
        Task<object> GetSuppliers();
    }
}
