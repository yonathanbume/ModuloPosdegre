using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IRequirementSupplierService
    {
        Task<int> Count();
        Task<RequirementSupplier> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<RequirementSupplier>> GetRequirementSuppliersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Delete(RequirementSupplier requirementSupplier);
        Task Insert(RequirementSupplier requirementSupplier);
        Task Update(RequirementSupplier requirementSupplier);
        Task AddAsync(RequirementSupplier requirementSupplier);
        IQueryable<RequirementSupplier> GetQueryWithData(Guid id);
        Task<object> GetSelectSupplier(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplierDatatable(DataTablesStructs.SentParameters sentParameters, Guid urquid, string search);
        Task<bool> AnySupplierId(Guid supplierId, Guid requerimentId);
    }
}
