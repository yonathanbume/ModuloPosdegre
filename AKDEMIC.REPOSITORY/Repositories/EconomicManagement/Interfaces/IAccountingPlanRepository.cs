using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IAccountingPlanRepository : IRepository<AccountingPlan>
    {
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Update();
        Task<DataTablesStructs.ReturnedData<object>> GetAccountingPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetAccountingPlansSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<object> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false);
    }
}
