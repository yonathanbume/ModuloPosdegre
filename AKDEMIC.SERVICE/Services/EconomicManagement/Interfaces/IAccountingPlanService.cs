using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IAccountingPlanService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAccountingPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task Insert(AccountingPlan entity);
        Task Update(AccountingPlan entity);
        Task Update();
        Task<AccountingPlan> Get(Guid id);
        Task<IEnumerable<AccountingPlan>> GetAll();
        Task Delete(AccountingPlan entity);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<IEnumerable<Select2Structs.Result>> GetAccountingPlansSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task InsertRange(IEnumerable<AccountingPlan> entities);
        Task Add(AccountingPlan accountingPlan);
        Task<object> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false);
    }
}
