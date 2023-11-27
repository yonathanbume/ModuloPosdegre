using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Income;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IIncomeService
    {
        Task InsertIncome(Income income);
        Task UpdateIncome(Income income);
        Task DeleteIncome(Income income);
        Task DeleteRange(List<Income> incomes);
        Task<Income> GetIncomeById(Guid id);
        Task<IEnumerable<Income>> GetAllIncomes();
        Task AddRangeAsync(List<Income> incomes);
        Task<DataTablesStructs.ReturnedData<DailyIncomeTemplate>> GetDailyIncomeDatatableClientSide(DateTime startDate, DateTime endDate, Guid? dependencyId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncomesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters,int year, Guid? dependencyId = null);
        Task<IEnumerable<Income>> GetAllByDependency(Guid dependencyId);
        Task InsertRange(List<Income> incomes);
        Task<List<Income>> GetByInvoiceId(Guid invoiceId);
    }
}
