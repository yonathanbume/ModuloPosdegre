using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerRetirementSystemHistoryService
    {
        Task<WorkerRetirementSystemHistory> GetActiveRetirementSystem(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerRetirementSystemHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task InsertWorkerRetirementSystem(WorkerRetirementSystemHistory entity);
        Task<bool> AnyActiveByUser(string userId, Guid? ignoredId = null);
        Task<WorkerRetirementSystemHistory> Get(Guid id);

        Task Update(WorkerRetirementSystemHistory entity);
    }
}
