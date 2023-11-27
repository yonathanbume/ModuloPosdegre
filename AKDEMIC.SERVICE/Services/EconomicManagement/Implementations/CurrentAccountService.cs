using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CurrentAccountService : ICurrentAccountService
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;

        public CurrentAccountService(ICurrentAccountRepository currentAccountRepository)
        {
            _currentAccountRepository = currentAccountRepository;
        }

        public Task DeleteById(Guid id) => _currentAccountRepository.DeleteById(id);

        public Task<CurrentAccount> Get(Guid id) => _currentAccountRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetCurrentAccountsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => _currentAccountRepository.GetCurrentAccountsDatatable(sentParameters, search);

        public Task<IEnumerable<Select2Structs.Result>> GettCurrentAccountsSelect2ClientSide()
            => _currentAccountRepository.GettCurrentAccountsSelect2ClientSide();

        public Task Insert(CurrentAccount entity) => _currentAccountRepository.Insert(entity);
        public async Task Add(CurrentAccount currentAccount) => await _currentAccountRepository.Add(currentAccount);
        public Task Update(CurrentAccount entity) => _currentAccountRepository.Update(entity);

        public async Task<IEnumerable<CurrentAccount>> GetAll()
            => await _currentAccountRepository.GetAll();

        public async Task Update()
            => await _currentAccountRepository.Update();

        public async Task<object> GetDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
            => await _currentAccountRepository.GetDatatableReport(sentParameters, search, startDate, endDate, type);
    }
}
