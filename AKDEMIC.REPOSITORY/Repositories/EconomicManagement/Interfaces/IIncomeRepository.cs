using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Income;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IIncomeRepository : IRepository<Income>
    {
        Task<DataTablesStructs.ReturnedData<DailyIncomeTemplate>> GetDailyIncomeDatatableClientSide(DateTime startDate, DateTime endDate, Guid? dependencyId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIncomesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters,int year, Guid? dependencyId = null);
        Task<IEnumerable<Income>> GetAllByDependency(Guid dependencyId);
        Task<List<Income>> GetByInvoiceId(Guid invoiceId);
    }
}
