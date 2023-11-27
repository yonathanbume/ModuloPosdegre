using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface IKardexRepository : IRepository<CafeteriaKardex>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetKardexDataTable(DataTablesStructs.SentParameters sentParameters, int type, DateTime startDate, DateTime endDate, Guid? ProviderId, string search);
    }
}
