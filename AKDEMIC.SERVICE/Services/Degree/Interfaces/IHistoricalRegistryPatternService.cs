using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Interfaces
{
    public interface IHistoricalRegistryPatternService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricalRegistryPatternsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(HistoricalRegistryPattern historicalRegistryPattern);
        Task<HistoricalRegistryPattern> Get(Guid id);
        Task Update(HistoricalRegistryPattern entity);
        Task Delete(HistoricalRegistryPattern entity);
    }
}
