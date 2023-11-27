using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollWorkerWageItemRepository : IRepository<PayrollWorkerWageItem>
    {
        Task<IEnumerable<PayrollWorkerWageItem>> GetAllByPayrollWorkerId(Guid payrollWorkerId);
        Task DeleteByPayrollWorkerId(Guid payrollWorkerId);
    }
}
