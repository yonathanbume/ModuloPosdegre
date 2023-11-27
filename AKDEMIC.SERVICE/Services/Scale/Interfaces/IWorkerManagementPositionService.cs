using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerManagementPositionService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerManagementPositionDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null);
        Task<IEnumerable<WorkerManagementPosition>> GetAll(string search, bool? onlyActive);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<bool> AnyByName(string name, Guid dependencyId ,Guid? ignoredId = null);
        Task Insert(WorkerManagementPosition entity);
        Task Update(WorkerManagementPosition entity);
        Task<WorkerManagementPosition> Get(Guid id);
        Task Delete(WorkerManagementPosition entity);
    }
}
