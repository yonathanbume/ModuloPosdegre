using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface ISoftwareTypeRepository : IRepository<SoftwareType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSoftwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
