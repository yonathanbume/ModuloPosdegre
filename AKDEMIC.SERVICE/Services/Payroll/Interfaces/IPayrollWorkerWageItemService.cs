using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollWorkerWageItemService
    {
        Task DeleteById(Guid payrollWorkerWageItemId);
        Task Get(Guid payrollWorkerWageItemId);
        Task Insert(PayrollWorkerWageItem payrollWorkerWageItem);
        Task InsertRange(IEnumerable<PayrollWorkerWageItem> payrollWorkerWageItems);
        Task<IEnumerable<PayrollWorkerWageItem>> GetByPayrollWorkerId(Guid payrollWorkerId);
    }
}
