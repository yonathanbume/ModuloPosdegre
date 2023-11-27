using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyTelephoneService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTransparencyTelephoneDataTable(DataTablesStructs.SentParameters sentParameters);
        Task AddRange(IEnumerable<TransparencyTelephone> entites);
    }
}
