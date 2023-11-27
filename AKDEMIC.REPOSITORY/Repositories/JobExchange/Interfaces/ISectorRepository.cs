using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ISectorRepository : IRepository<Sector>
    {
        Task<object> GetSectorsSelect2(Guid? sectorId);
        Task<DataTablesStructs.ReturnedData<object>> GetSectorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetSectorsSelect2V2();
    }
}
