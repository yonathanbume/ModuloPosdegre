using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerLaborConditionRepository : IRepository<WorkerLaborCondition>
    {
        Task<IEnumerable<WorkerLaborCondition>> GetAllWithIncludes(int? status = null);
        Task<object> GetAllAsSelect2ClientSide(bool includeTitle = false);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborConditionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null);
    }
}