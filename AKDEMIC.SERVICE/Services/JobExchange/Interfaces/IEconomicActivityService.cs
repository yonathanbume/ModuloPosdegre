using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IEconomicActivityService
    {
        Task<object> GetClientSideSelect2(Guid? economicActivityDivisionId = null);
        Task<bool> ExistCode(string code, Guid? Id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEcnomicActivityDatatable(DataTablesStructs.SentParameters sentParameters,Guid? economicActivityDivisionId = null, string searchValue = null);
        Task Insert(EconomicActivity model);
        Task Update(EconomicActivity model);
        Task Delete(EconomicActivity model);
        Task<EconomicActivity> Get(Guid id);
    }
}
