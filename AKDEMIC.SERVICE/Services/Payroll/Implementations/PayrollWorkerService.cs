using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PayrollWorkerService : IPayrollWorkerService
    {
        private readonly IPayrollWorkerRepository _payrollWorkerRepository;

        public PayrollWorkerService(IPayrollWorkerRepository payrollWorkerRepository)
        {
            _payrollWorkerRepository = payrollWorkerRepository;
        }

        public async Task DeleteById(Guid id)
            => await _payrollWorkerRepository.DeleteById(id);

        public async Task DeleteByPayrollId(Guid payrollId)
            => await _payrollWorkerRepository.DeleteByPayrollId(payrollId);

        public async Task<IEnumerable<PayrollWorker>> GetAllByPayrollId(Guid payrollId)
            => await _payrollWorkerRepository.GetAllByPayrollId(payrollId);

        public async Task<IEnumerable<Guid>> GetIdsByPayrollId(Guid payrollId)
            => await _payrollWorkerRepository.GetIdsByPayrollId(payrollId);

        public async Task<(IList<PayrollWorker> pagedList, int count)> GetPayrollWorkersByPayrollIdAndPaginationParameter(Guid payrollId, PaginationParameter paginationParameter)
            => await _payrollWorkerRepository.GetPayrollWorkersByPayrollIdAndPaginationParameter(payrollId, paginationParameter);

        public async Task InsertRange(IEnumerable<PayrollWorker> payrollWorkers)
            => await _payrollWorkerRepository.InsertRange(payrollWorkers);

        public async Task UpdateRange(IEnumerable<PayrollWorker> payrollWorkers)
            => await _payrollWorkerRepository.UpdateRange(payrollWorkers);
    }
}
