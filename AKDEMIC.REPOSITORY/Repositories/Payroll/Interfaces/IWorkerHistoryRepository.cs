using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkerHistoryRepository : IRepository<WorkerHistory>
    {
        Task<(IEnumerable<WorkerHistory> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter, Guid workerId);
        Task FireWorkerById(Guid workerId);
        Task RestoreWorkerById(Guid workerId);
    }
}
