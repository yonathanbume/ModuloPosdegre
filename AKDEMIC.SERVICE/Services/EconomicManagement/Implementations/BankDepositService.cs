using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class BankDepositService : IBankDepositService
    {
        private readonly IBankDepositRepository _bankDepositRepository;

        public BankDepositService(IBankDepositRepository bankDepositRepository)
        {
            _bankDepositRepository = bankDepositRepository;
        }
        public async Task InsertRange(List<BankDeposit> bankDeposit)
            => await _bankDepositRepository.InsertRange(bankDeposit);
        public async Task InsertBankDeposit(BankDeposit bankDeposit) =>
            await _bankDepositRepository.Insert(bankDeposit);

        public async Task UpdateBankDeposit(BankDeposit bankDeposit) =>
            await _bankDepositRepository.Update(bankDeposit);

        public async Task DeleteBankDeposit(BankDeposit bankDeposit) =>
            await _bankDepositRepository.Delete(bankDeposit);

        public async Task<BankDeposit> GetBankDepositById(Guid id) =>
            await _bankDepositRepository.Get(id);

        public async Task<IEnumerable<BankDeposit>> GetAllBankDeposits() =>
            await _bankDepositRepository.GetAll();
        public async Task<int> Count()
            => await _bankDepositRepository.Count();
        public async Task<DataTablesStructs.ReturnedData<object>> GetBankDepositDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _bankDepositRepository.GetBankDepositDatatable(sentParameters, search);
        public async Task<object> GetObjectById(Guid id)
            => await _bankDepositRepository.GetObjectById(id);

        public async Task<BankDeposit> Get(Guid id)
            => await _bankDepositRepository.Get(id);

        public async Task<List<BankDeposit>> GetAllByPettyCashBookId(Guid pettyCashBookId)
            => await _bankDepositRepository.GetAllByPettyCashBookId(pettyCashBookId);
    }
}
