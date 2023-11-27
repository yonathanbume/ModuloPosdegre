using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerManagementPositionRepository : IRepository<WorkerManagementPosition>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerManagementPositionDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null);
        Task<IEnumerable<WorkerManagementPosition>> GetAll(string search, bool? onlyActive);
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<bool> AnyByName(string name, Guid dependencyId ,Guid? ignoredId = null);
    }
}
