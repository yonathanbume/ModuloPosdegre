using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface IKardexService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetKardexDataTable(DataTablesStructs.SentParameters sentParameters, int type, DateTime startDate, DateTime endDate, Guid? ProviderId, string search);
        Task AddRange(IEnumerable<CafeteriaKardex> cafeteriaKardexes);
        Task UpdateRange(IEnumerable<CafeteriaKardex> cafeteriaKardexes);

    }
}
