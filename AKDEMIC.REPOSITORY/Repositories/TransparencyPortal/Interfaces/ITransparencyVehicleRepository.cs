using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface ITransparencyVehicleRepository : IRepository<TransparencyVehicle>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyVehicleDataTable(DataTablesStructs.SentParameters sentParameters);
    }
}
