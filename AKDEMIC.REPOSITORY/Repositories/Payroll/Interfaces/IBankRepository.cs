using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IBankRepository : IRepository<Bank>
    {
        Task<object> GetBanks();

        Task<DataTablesStructs.ReturnedData<object>> GetAllBanksDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
