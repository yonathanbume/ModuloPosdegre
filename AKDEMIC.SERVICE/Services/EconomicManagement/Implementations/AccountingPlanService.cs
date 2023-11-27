using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class AccountingPlanService : IAccountingPlanService
    {
        private readonly IAccountingPlanRepository _accountingPlanRepository;

        public AccountingPlanService(IAccountingPlanRepository accountingPlanRepository)
        {
            _accountingPlanRepository = accountingPlanRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _accountingPlanRepository.AnyByCode(code, ignoredId);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _accountingPlanRepository.AnyByName(name, ignoredId);

        public async Task Delete(AccountingPlan entity)
            => await _accountingPlanRepository.Delete(entity);

        public async Task<AccountingPlan> Get(Guid id)
            => await _accountingPlanRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAccountingPlansDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _accountingPlanRepository.GetAccountingPlansDatatable(sentParameters, search);

        public async Task<IEnumerable<Select2Structs.Result>> GetAccountingPlansSelect2ClientSide()
            => await _accountingPlanRepository.GetAccountingPlansSelect2ClientSide();

        public async Task Insert(AccountingPlan entity)
            => await _accountingPlanRepository.Insert(entity);

        public async Task Update(AccountingPlan entity)
            => await _accountingPlanRepository.Update(entity);

        public async Task InsertRange(IEnumerable<AccountingPlan> entities)
            => await _accountingPlanRepository.InsertRange(entities);
        public async Task Add(AccountingPlan accountingPlan)
            => await _accountingPlanRepository.Add(accountingPlan);

        public async Task<IEnumerable<AccountingPlan>> GetAll()
            => await _accountingPlanRepository.GetAll();

        public async Task Update()
            => await _accountingPlanRepository.Update();

        public async Task<object> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false)
            => await _accountingPlanRepository.GetReportDatatable(sentParameters, search, startDate, endDate, showAll);
    }
}
