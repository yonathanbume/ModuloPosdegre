using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ISiafExpenseRepository : IRepository<SiafExpense>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSiafExpenseDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDerivedExpensesDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
