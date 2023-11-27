using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IBankDepositRepository : IRepository<BankDeposit>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetBankDepositDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetObjectById(Guid id);
        Task<List<BankDeposit>> GetAllByPettyCashBookId(Guid pettyCashBookId);
        //Task<SelectList> GetTypeCatalog();
        //Task<bool> AnyByCode(string code);
    }
}
