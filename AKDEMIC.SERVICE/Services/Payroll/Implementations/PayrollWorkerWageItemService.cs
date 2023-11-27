using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PayrollWorkerWageItemService : IPayrollWorkerWageItemService
    {
        private readonly IPayrollWorkerWageItemRepository _payrollWorkerWageItemRepository;

        public PayrollWorkerWageItemService(IPayrollWorkerWageItemRepository payrollWorkerWageItemRepository)
        {
            _payrollWorkerWageItemRepository = payrollWorkerWageItemRepository;
        }

        public async Task DeleteById(Guid payrollWorkerWageItemId)
            => await _payrollWorkerWageItemRepository.DeleteById(payrollWorkerWageItemId);

        public async Task Get(Guid payrollWorkerWageItemId)
            => await _payrollWorkerWageItemRepository.Get(payrollWorkerWageItemId);

        public async Task<IEnumerable<PayrollWorkerWageItem>> GetByPayrollWorkerId(Guid payrollWorkerId)
            => await _payrollWorkerWageItemRepository.GetAllByPayrollWorkerId(payrollWorkerId);

        public async Task Insert(PayrollWorkerWageItem payrollWorkerWageItem)
            => await _payrollWorkerWageItemRepository.Insert(payrollWorkerWageItem);

        public async Task InsertRange(IEnumerable<PayrollWorkerWageItem> payrollWorkerWageItems)
            => await _payrollWorkerWageItemRepository.InsertRange(payrollWorkerWageItems);
    }
}
