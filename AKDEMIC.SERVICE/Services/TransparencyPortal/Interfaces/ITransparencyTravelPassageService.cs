using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyTravelPassageService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTravelPassageDataTable(DataTablesStructs.SentParameters sentParameters);
        Task InsertRange(IEnumerable<TransparencyTravelPassage> entities);
    }
}
