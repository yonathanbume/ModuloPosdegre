using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;

        public PayrollService(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task Delete(ENTITIES.Models.Payroll.Payroll payroll)
            => await _payrollRepository.Delete(payroll);

        public async Task DeleteById(Guid id) => await _payrollRepository.DeleteById(id);

        public async Task<ENTITIES.Models.Payroll.Payroll> FindByCode(string code)
            => await _payrollRepository.FindByCode(code);

        public async Task<ENTITIES.Models.Payroll.Payroll> Get(Guid id) => await _payrollRepository.Get(id);

        public async Task<IEnumerable<ENTITIES.Models.Payroll.Payroll>> GetAll() => await _payrollRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _payrollRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task<(IEnumerable<ENTITIES.Models.Payroll.Payroll> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
            => await _payrollRepository.GetAllByPaginationParameter(paginationParameter);

        public async Task Insert(ENTITIES.Models.Payroll.Payroll payroll) => await _payrollRepository.Insert(payroll);

        public async Task Update(ENTITIES.Models.Payroll.Payroll payroll) => await _payrollRepository.Update(payroll);
    }
}
