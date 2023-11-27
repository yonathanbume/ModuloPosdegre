using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces
{
    public interface IHistoricalRegistryPatternRepository : IRepository<HistoricalRegistryPattern>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricalRegistryPatternsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
