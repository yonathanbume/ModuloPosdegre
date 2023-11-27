using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ISupplyPackageRepository : IRepository<SupplyPackage>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSupplyPackages(DataTablesStructs.SentParameters sentParameters,string searchValue = null);
        Task<object> GetSupplyPackageSelect();
    }
}
