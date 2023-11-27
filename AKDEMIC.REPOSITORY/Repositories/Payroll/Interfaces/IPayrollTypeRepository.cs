using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollTypeRepository : IRepository<PayrollType>
    {
        Task<(IEnumerable<PayrollType> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<PayrollType> FindByCode(string code);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<object> GetPayrollTypesJson(string term);
        Task<bool> AnyByCode(string code, Guid? id = null);
    }
}
