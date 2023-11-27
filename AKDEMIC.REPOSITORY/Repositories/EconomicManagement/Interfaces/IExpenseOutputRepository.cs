using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IExpenseOutputRepository : IRepository<ExpenseOutput>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsDatatables(DataTablesStructs.SentParameters sentParameters, string userId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsTesoDatatables(DataTablesStructs.SentParameters sentParameters, string search);
        Task<List<ExpenseOutput>> GetExpenseOutputReportList(Guid id);
    }
}
