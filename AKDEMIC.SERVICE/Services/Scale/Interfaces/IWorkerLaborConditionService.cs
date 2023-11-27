using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerLaborConditionService
    {
        Task<WorkerLaborCondition> Get(Guid id);
        Task<object> GetAllAsSelect2ClientSide(bool includeTitle = false);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborConditionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null);
        Task Insert(WorkerLaborCondition entity);
        Task Update(WorkerLaborCondition entity);
        Task Delete(WorkerLaborCondition entity);
        Task<IEnumerable<WorkerLaborCondition>> GetAll();
        Task<IEnumerable<WorkerLaborCondition>> GetAllWithIncludes(int? status = null);
    }
}