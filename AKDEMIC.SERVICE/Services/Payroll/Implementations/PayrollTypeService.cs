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
    public class PayrollTypeService : IPayrollTypeService
    {
        private readonly IPayrollTypeRepository _payrollTypeRepository;

        public PayrollTypeService(IPayrollTypeRepository payrollTypeRepository)
        {
            _payrollTypeRepository = payrollTypeRepository;
        }

        public async Task<PayrollType> FindByCode(string code)
            => await _payrollTypeRepository.FindByCode(code);

        public async Task DeleteById(Guid id) 
            => await _payrollTypeRepository.DeleteById(id);

        public async Task<PayrollType> Get(Guid id)
            => await _payrollTypeRepository.Get(id);

        public async Task<IEnumerable<PayrollType>> GetAll()
            => await _payrollTypeRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _payrollTypeRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task<(IEnumerable<PayrollType> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
            => await _payrollTypeRepository.GetAllByPaginationParameter(paginationParameter);

        public async Task Insert(PayrollType payrollType)
            => await _payrollTypeRepository.Insert(payrollType);

        public async Task Update(PayrollType payrollType)
            => await _payrollTypeRepository.Update(payrollType);

        public async Task<object> GetPayrollTypesJson(string term)
            => await _payrollTypeRepository.GetPayrollTypesJson(term);

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _payrollTypeRepository.AnyByCode(code, id);
    }
}
