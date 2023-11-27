using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IComputerSupplierRepository:IRepository<ComputerSupplier>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetComputerSupplierDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
