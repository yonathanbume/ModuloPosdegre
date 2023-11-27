using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ISupplierCategoryRepository : IRepository<SupplierCategory>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSupplierCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSuppliersReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? supplierCategoryId = null, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? id = null);
    }
}
