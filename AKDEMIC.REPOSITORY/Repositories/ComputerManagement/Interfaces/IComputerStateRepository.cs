using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IComputerStateRepository:IRepository<ComputerState>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetComputerStateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
