using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<bool> AnySupplierByName(string name);
        Task<bool> AnySupplierByName(string name, string ruc);
        Task<bool> AnySupplierByRUC(string ruc);
        Task<bool> AnySupplierByNameDistint(string name, string ruc, Guid id);
        Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Supplier>> GetSuppliersDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<object> GetSuppliers();
    }
}
