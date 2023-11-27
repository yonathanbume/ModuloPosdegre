using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IEconomicActivitySectionRepository : IRepository<EconomicActivitySection>
    {
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<object> GetClientSideSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
