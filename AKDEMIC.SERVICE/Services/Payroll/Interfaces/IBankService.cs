using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IBankService
    {
        Task<IEnumerable<Bank>> GetAll();

        Task<object> GetBanks();

        Task<Bank> Get(Guid id);

        Task Insert(Bank bank);

        Task Update(Bank bank);

        Task Delete(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetAllBanksDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
