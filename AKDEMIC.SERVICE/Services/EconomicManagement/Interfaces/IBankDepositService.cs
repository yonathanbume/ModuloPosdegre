using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IBankDepositService
    {
        Task<BankDeposit> Get(Guid id);
        Task InsertRange(List<BankDeposit> catalogItem);
        Task InsertBankDeposit(BankDeposit catalogItem);
        Task UpdateBankDeposit(BankDeposit catalogItem);
        Task DeleteBankDeposit(BankDeposit catalogItem);
        Task<List<BankDeposit>> GetAllByPettyCashBookId(Guid pettyCashBookId);
        Task<BankDeposit> GetBankDepositById(Guid id);
        Task<IEnumerable<BankDeposit>> GetAllBankDeposits();
        Task<int> Count();
        Task<DataTablesStructs.ReturnedData<object>> GetBankDepositDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetObjectById(Guid id);
        //Task<SelectList> GetTypeCatalog();
        //Task<bool> AnyByCode(string code);
    }
}
