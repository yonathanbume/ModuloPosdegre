using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ISectorService
    {
        Task<object> GetSectorsSelect2(Guid? sectorId);
        Task<IEnumerable<Sector>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetSectorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(Sector model);
        Task Update(Sector model);
        Task Delete(Sector model);
        Task<Sector> Get(Guid id);
        Task<object> GetSectorsSelect2V2();
    }
}
