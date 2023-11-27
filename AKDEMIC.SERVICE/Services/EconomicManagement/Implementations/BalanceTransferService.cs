using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class BalanceTransferService : IBalanceTransferService
    {
        private readonly IBalanceTransferRepository _balanceTransferRepository;

        public BalanceTransferService(IBalanceTransferRepository balanceTransferRepository)
        {
            _balanceTransferRepository = balanceTransferRepository;
        }

        public async Task InsertBalanceTransfer(BalanceTransfer balanceTransfer) =>
            await _balanceTransferRepository.Insert(balanceTransfer);

        public async Task UpdateBalanceTransfer(BalanceTransfer balanceTransfer) =>
            await _balanceTransferRepository.Update(balanceTransfer);

        public async Task DeleteBalanceTransfer(BalanceTransfer balanceTransfer) =>
            await _balanceTransferRepository.Delete(balanceTransfer);

        public async Task<BalanceTransfer> GetBalanceTransferById(Guid id) =>
            await _balanceTransferRepository.Get(id);

        public async Task<IEnumerable<BalanceTransfer>> GetAllBalanceTransfers() =>
            await _balanceTransferRepository.GetAllBalanceTransfers();
        public  IQueryable<BalanceTransfer> TransfersQry(DateTime date)
            =>  _balanceTransferRepository.TransfersQry(date);
        public async Task<DataTablesStructs.ReturnedData<object>> GetBalanceTransferDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _balanceTransferRepository.GetBalanceTransferDatatable(sentParameters, search);

        public async Task DeleteById(Guid id) => await _balanceTransferRepository.DeleteById(id);
    }
}
