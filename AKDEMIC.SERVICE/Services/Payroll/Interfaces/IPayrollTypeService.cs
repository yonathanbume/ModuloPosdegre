using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollTypeService
    {
        Task<IEnumerable<PayrollType>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<(IEnumerable<PayrollType> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);
        Task<PayrollType> Get(Guid id);
        Task<PayrollType> FindByCode(string code);
        Task Insert(PayrollType payrollType);
        Task Update(PayrollType payrollType);
        Task DeleteById(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<object> GetPayrollTypesJson(string term);
    }
}
