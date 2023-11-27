using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface  IEconomicActivityRepository : IRepository<EconomicActivity>
    {
        Task<object> GetClientSideSelect2(Guid? economicActivityDivisionId = null);
        Task<bool> ExistCode(string code, Guid? Id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEcnomicActivityDatatable(DataTablesStructs.SentParameters sentParameters, Guid? economicActivityDivisionId = null, string searchValue = null);
    }
}
