using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IStructureForExpenseService
    {
        Task InsertStructureForExpense(StructureForExpense structureForExpense);
        Task UpdateStructureForExpense(StructureForExpense structureForExpense);
        Task DeleteStructureForExpense(StructureForExpense structureForExpense);
        Task<StructureForExpense> GetStructureForExpenseById(Guid id);
        Task<IEnumerable<StructureForExpense>> GetAllStructureForExpenses();
        Task<int> Count();
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetStructureForExpensesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year);
        Task InsertRange(List<StructureForExpense> structureForExpense);
    }
}
