using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollClassWageItemFormulaRepository : IRepository<PayrollClassWageItemFormula>
    {
        Task<IEnumerable<PayrollClassWageItemFormula>> GetAllByWageItem(Guid wageItemId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid wageItemId,string searchValue = null);

        Task<(IEnumerable<PayrollClassWageItemFormula> pagedList, int count)> GetAllByWageItemAndPaginationParameter(Guid wageItemId, PaginationParameter paginationParameter);
    }
}
