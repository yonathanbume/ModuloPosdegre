using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollClassWageItemFormulaService
    {
        Task<PayrollClassWageItemFormula> Get(Guid id);
        Task Insert(PayrollClassWageItemFormula payrollClassWageItemFormula);
        Task Update(PayrollClassWageItemFormula payrollClassWageItemFormula);
        Task Delete(PayrollClassWageItemFormula payrollClassWageItemFormula);
        Task DeleteById(Guid id);
        Task<IEnumerable<PayrollClassWageItemFormula>> GetAllByWageItem(Guid wageItemId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid wageItemId, string searchValue = null);
        Task<(IEnumerable<PayrollClassWageItemFormula> pagedList, int count)> GetAllByWageItemAndPaginationParameter(Guid wageItemId, PaginationParameter paginationParameter);
    }
}
