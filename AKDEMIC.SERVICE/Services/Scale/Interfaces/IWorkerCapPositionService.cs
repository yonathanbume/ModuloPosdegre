using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerCapPositionService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerCapPositionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue);
        Task<IEnumerable<WorkerCapPosition>> GetAll(string search = null, bool? onlyActive = false);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Insert(WorkerCapPosition entity);
        Task Update(WorkerCapPosition entity);
        Task Delete(WorkerCapPosition entity);
        Task<WorkerCapPosition> Get(Guid id);
    }
}
