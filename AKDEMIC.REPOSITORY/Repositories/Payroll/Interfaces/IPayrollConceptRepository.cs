using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollConceptRepository : IRepository<PayrollConcept>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllPayrollConceptsDatatable(DataTablesStructs.SentParameters sentParameters, int type, string searchValue = null);

        Task<bool> AnyByCode(string code, Guid? id = null);
    }
}
