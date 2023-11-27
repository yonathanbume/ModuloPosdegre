using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ICurrentAccountRepository : IRepository<CurrentAccount>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCurrentAccountsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GettCurrentAccountsSelect2ClientSide();
        Task Update();
        Task<object> GetDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
    }
}
