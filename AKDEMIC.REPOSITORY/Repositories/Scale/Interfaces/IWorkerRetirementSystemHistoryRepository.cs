using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerRetirementSystemHistoryRepository : IRepository<WorkerRetirementSystemHistory>
    {
        Task<WorkerRetirementSystemHistory> GetActiveRetirementSystem(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerRetirementSystemHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task InsertWorkerRetirementSystem(WorkerRetirementSystemHistory entity);
        Task<bool> AnyActiveByUser(string userId, Guid? ignoredId = null);
    }
}
