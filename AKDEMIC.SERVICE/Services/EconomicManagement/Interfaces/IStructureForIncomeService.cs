using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IStructureForIncomeService
    {
        Task InsertStructureForIncome(StructureForIncome structureForIncome);
        Task UpdateStructureForIncome(StructureForIncome structureForIncome);
        Task DeleteStructureForIncome(StructureForIncome structureForIncome);
        Task<StructureForIncome> GetStructureForIncomeById(Guid id);
        Task<IEnumerable<StructureForIncome>> GetAllStructureForIncomes();
        Task<int> Count();
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetStructureForIncomesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year, string userId);
        Task InsertRange(List<StructureForIncome> structureForIncome);
    }
}
