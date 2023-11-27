using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollWorkerRepository : IRepository<PayrollWorker>
    {
        Task DeleteByPayrollId(Guid payrollId);
        Task<IEnumerable<PayrollWorker>> GetAllByPayrollId(Guid payrollId);
        Task<IEnumerable<Guid>> GetIdsByPayrollId(Guid payrollId);
        Task<(IList<PayrollWorker> pagedList, int count)> GetPayrollWorkersByPayrollIdAndPaginationParameter(Guid payrollId, PaginationParameter paginationParameter);
    }
}
