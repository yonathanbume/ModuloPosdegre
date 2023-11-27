using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IComputerTypeRepository : IRepository<ComputerType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetComputerTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
