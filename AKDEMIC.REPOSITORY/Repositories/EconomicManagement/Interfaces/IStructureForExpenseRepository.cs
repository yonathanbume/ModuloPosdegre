using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IStructureForExpenseRepository : IRepository<StructureForExpense>
    {
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetStructureForExpensesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year);
    }
}
