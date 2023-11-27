using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerCapPositionRepository : IRepository<WorkerCapPosition>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerCapPositionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue);
        Task<IEnumerable<WorkerCapPosition>> GetAll(string search = null, bool? onlyActive = false);
        Task<bool> AnyByCode(string code, Guid? ignoreId = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
