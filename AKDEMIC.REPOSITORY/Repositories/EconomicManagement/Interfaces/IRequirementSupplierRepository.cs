using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IRequirementSupplierRepository : IRepository<RequirementSupplier>
    {
        Task<DataTablesStructs.ReturnedData<RequirementSupplier>> GetRequirementSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        IQueryable<RequirementSupplier> GetQueryWithData(Guid id);
        Task<object> GetSelectSupplier(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplierDatatable(DataTablesStructs.SentParameters sentParameters, Guid urquid, string search);
        Task<bool> AnySupplierId(Guid supplierId, Guid requerimentId);
    }
}
