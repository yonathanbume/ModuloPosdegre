using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollWorkerService
    {
        Task InsertRange(IEnumerable<PayrollWorker> payrollWorkers);
        Task UpdateRange(IEnumerable<PayrollWorker> payrollWorkers);
        Task DeleteById(Guid id);
        Task DeleteByPayrollId(Guid payrollId);
        Task<IEnumerable<PayrollWorker>> GetAllByPayrollId(Guid payrollId);
        Task<IEnumerable<Guid>> GetIdsByPayrollId(Guid payrollId);
        Task<(IList<PayrollWorker> pagedList, int count)> GetPayrollWorkersByPayrollIdAndPaginationParameter(Guid payrollId, PaginationParameter paginationParameter);
    }
}
