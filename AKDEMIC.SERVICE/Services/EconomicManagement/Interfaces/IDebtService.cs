using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IDebtService
    {
        Task InsertDebt(Debt debt);
        Task UpdateDebt(Debt debt);
        Task DeleteDebt(Debt debt);
        Task<Debt> GetDebtById(Guid id);
        Task<IEnumerable<Debt>> GetAllDebts();
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
    }
}
