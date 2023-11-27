using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task Delete(Guid id) => await _bankRepository.DeleteById(id);

        public async Task<Bank> Get(Guid id) => await _bankRepository.Get(id);

        public async Task<IEnumerable<Bank>> GetAll() => await _bankRepository.GetAll();

        public async Task<object> GetBanks()
            => await _bankRepository.GetBanks();

        public async Task Insert(Bank bank) => await _bankRepository.Insert(bank);

        public async Task Update(Bank bank) => await _bankRepository.Update(bank);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllBanksDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                    => await _bankRepository.GetAllBanksDatatable(sentParameters, searchValue);


    }
}
