using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICurrentAccountService
    {
        Task Insert(CurrentAccount entity);
        Task Add(CurrentAccount currentAccount);
        Task Update(CurrentAccount entity);
        Task Update();
        Task<CurrentAccount> Get(Guid id);
        Task<IEnumerable<CurrentAccount>> GetAll();
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetCurrentAccountsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GettCurrentAccountsSelect2ClientSide();
        Task<object> GetDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null);
    }
}
