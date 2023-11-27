using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IEconomicActivitySectionService
    {
        Task<EconomicActivitySection> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task Insert(EconomicActivitySection economicActivitySection);
        Task Update(EconomicActivitySection economicActivitySection);
        Task Delete(EconomicActivitySection economicActivitySection);
        Task<object> GetClientSideSelect2();

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
