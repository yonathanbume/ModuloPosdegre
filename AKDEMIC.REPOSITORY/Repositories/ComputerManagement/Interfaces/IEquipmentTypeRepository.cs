using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IEquipmentTypeRepository:IRepository<EquipmentType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
