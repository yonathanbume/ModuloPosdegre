using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyVehicleService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyVehicleDataTable(DataTablesStructs.SentParameters sentParameters);
        Task InsertRange(IEnumerable<TransparencyVehicle> entities);
    }
}
