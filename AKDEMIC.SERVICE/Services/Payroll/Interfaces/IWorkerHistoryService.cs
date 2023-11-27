using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkerHistoryService
    {
        Task<(IEnumerable<WorkerHistory> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter, Guid workerId);
        Task<IEnumerable<WorkerHistory>> GetAll();
        Task<WorkerHistory> Get(Guid id);
        Task Insert(WorkerHistory workerHistory);
        Task Update(WorkerHistory workerHistory);
        Task DeleteById(Guid id);
        Task FireWorkerById(Guid workerId);
        Task RestoreWorkerById(Guid workerId);
    }
}
