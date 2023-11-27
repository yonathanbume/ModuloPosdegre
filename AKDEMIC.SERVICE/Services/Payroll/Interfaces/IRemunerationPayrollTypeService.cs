using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IRemunerationPayrollTypeService
    {
        Task<IEnumerable<RemunerationPayrollType>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? payrollTypeId = null, string searchValue = null);
        Task<RemunerationPayrollType> Get(Guid id);
        Task<RemunerationPayrollType> GetWithIncludes(Guid id);
        Task Insert(RemunerationPayrollType remunerationPayrollType);
        Task Update(RemunerationPayrollType remunerationPayrollType);
        Task Delete(RemunerationPayrollType remunerationPayrollType);
    }
}
