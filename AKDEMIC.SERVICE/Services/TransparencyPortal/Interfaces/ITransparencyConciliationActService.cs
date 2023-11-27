using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyConciliationActService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters);
        Task InsertRange(List<TransparencyConciliationAct> part);
    }
}
