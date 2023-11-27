using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollClassService
    {
        Task<IEnumerable<PayrollClass>> GetAll();
        Task<(IEnumerable<PayrollClass> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<PayrollClass> Get(Guid id);
        Task<PayrollClass> FindByCode(string code);
        Task Insert(PayrollClass payrollClass);
        Task Update(PayrollClass payrollClass);
        Task DeleteById(Guid id);
    }
}
