using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollConceptService
    {
        Task<PayrollConcept> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<IEnumerable<PayrollConcept>> GetAll();
        Task Insert(PayrollConcept payrollConcept);
        Task Update(PayrollConcept payrollConcept);
        Task Delete(PayrollConcept payrollConcept);

        Task<DataTablesStructs.ReturnedData<object>> GetAllPayrollConceptsDatatable(DataTablesStructs.SentParameters sentParameters, int type,string searchValue = null);
    }
}
