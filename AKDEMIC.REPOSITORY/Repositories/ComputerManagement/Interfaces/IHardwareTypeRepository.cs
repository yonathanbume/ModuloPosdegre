using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IHardwareTypeRepository:IRepository<HardwareType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetHardwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
