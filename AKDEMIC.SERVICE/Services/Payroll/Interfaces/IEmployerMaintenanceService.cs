using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IEmployerMaintenanceService
    {
        Task<EmployerMaintenance> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);

        Task<IEnumerable<EmployerMaintenance>> GetAll();

        Task Insert(EmployerMaintenance employerMaintenance);
        Task Update(EmployerMaintenance employerMaintenance);
        Task Delete(EmployerMaintenance employerMaintenance);

        Task<DataTablesStructs.ReturnedData<object>> GetAllEmployerMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
