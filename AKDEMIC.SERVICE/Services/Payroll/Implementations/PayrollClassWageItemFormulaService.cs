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
    public class PayrollClassWageItemFormulaService : IPayrollClassWageItemFormulaService
    {
        private readonly IPayrollClassWageItemFormulaRepository _payrollClassWageItemFormulaRepository;

        public PayrollClassWageItemFormulaService(IPayrollClassWageItemFormulaRepository payrollClassWageItemFormulaRepository)
        {
            _payrollClassWageItemFormulaRepository = payrollClassWageItemFormulaRepository;
        }

        public async Task Delete(PayrollClassWageItemFormula payrollClassWageItemFormula)
            => await _payrollClassWageItemFormulaRepository.Delete(payrollClassWageItemFormula);

        public async Task DeleteById(Guid id)
            => await _payrollClassWageItemFormulaRepository.DeleteById(id);

        public async Task<PayrollClassWageItemFormula> Get(Guid id)
            => await _payrollClassWageItemFormulaRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid wageItemId, string searchValue = null)
            => _payrollClassWageItemFormulaRepository.GetAllDatatable(sentParameters, wageItemId,searchValue);

        public async Task<IEnumerable<PayrollClassWageItemFormula>> GetAllByWageItem(Guid wageItemId)
            => await _payrollClassWageItemFormulaRepository.GetAllByWageItem(wageItemId);

        public async Task<(IEnumerable<PayrollClassWageItemFormula> pagedList, int count)> GetAllByWageItemAndPaginationParameter(Guid wageItemId, PaginationParameter paginationParameter)
            => await _payrollClassWageItemFormulaRepository.GetAllByWageItemAndPaginationParameter(wageItemId, paginationParameter);

        public async Task Insert(PayrollClassWageItemFormula payrollClassWageItemFormula)
            => await _payrollClassWageItemFormulaRepository.Insert(payrollClassWageItemFormula);

        public async Task Update(PayrollClassWageItemFormula payrollClassWageItemFormula)
            => await _payrollClassWageItemFormulaRepository.Update(payrollClassWageItemFormula);
    }
}
