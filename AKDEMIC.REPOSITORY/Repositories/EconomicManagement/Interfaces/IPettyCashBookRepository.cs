using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IPettyCashBookRepository:IRepository<PettyCashBook>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetPettyCashBookDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
    }
}
