using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IPettyCashBookService
    {
        Task Insert(PettyCashBook book);
        Task<PettyCashBook> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetPettyCashBookDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
    }
}
