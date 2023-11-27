using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ISupplyPackageService
    {
        Task Insert(SupplyPackage model);
        Task Update(SupplyPackage model);
        Task Delete(SupplyPackage model);
        Task<SupplyPackage> Get(Guid Id);
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyPackages(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSupplyPackageSelect();
    }
}
