using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PayrollClassService : IPayrollClassService
    {
        private readonly IPayrollClassRepository _payrollClassRepository;

        public PayrollClassService(IPayrollClassRepository payrollClassRepository)
        {
            _payrollClassRepository = payrollClassRepository;
        }

        public async Task<PayrollClass> FindByCode(string code)
            => await _payrollClassRepository.FindByCode(code);

        public async Task DeleteById(Guid id)
            => await _payrollClassRepository.DeleteById(id);

        public async Task<PayrollClass> Get(Guid id)
            => await _payrollClassRepository.Get(id);

        public async Task<IEnumerable<PayrollClass>> GetAll()
            => await _payrollClassRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _payrollClassRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task<(IEnumerable<PayrollClass> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
            => await _payrollClassRepository.GetAllByPaginationParameter(paginationParameter);

        public async Task Insert(PayrollClass payrollClass)
            => await _payrollClassRepository.Insert(payrollClass);

        public async Task Update(PayrollClass payrollClass)
            => await _payrollClassRepository.Update(payrollClass);
    }
}
