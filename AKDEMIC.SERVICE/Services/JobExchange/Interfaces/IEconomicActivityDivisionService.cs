using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IEconomicActivityDivisionService
    {
        Task<EconomicActivityDivision> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task Insert(EconomicActivityDivision economicActivityDivision);
        Task Update(EconomicActivityDivision economicActivityDivision);
        Task Delete(EconomicActivityDivision economicActivityDivision);
        Task<object> GetClientSideSelect2(Guid? economicActivitySectionId = null);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivitySectionId = null, string searchValue = null);
    }
}
