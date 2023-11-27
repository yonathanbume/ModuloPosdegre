using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class EmployerMaintenanceService : IEmployerMaintenanceService
    {
        private readonly IEmployerMaintenanceRepository _employerMaintenance;

        public EmployerMaintenanceService(IEmployerMaintenanceRepository employerMaintenance)
        {
            _employerMaintenance = employerMaintenance;
        }

        public async Task Delete(EmployerMaintenance employerMaintenance)
            => await _employerMaintenance.Delete(employerMaintenance);

        public async Task<EmployerMaintenance> Get(Guid id)
            => await _employerMaintenance.Get(id);

        public async Task<IEnumerable<EmployerMaintenance>> GetAll()
            => await _employerMaintenance.GetAll();

        public async Task Insert(EmployerMaintenance employerMaintenance)
            => await _employerMaintenance.Insert(employerMaintenance);

        public async Task Update(EmployerMaintenance employerMaintenance)
            => await _employerMaintenance.Update(employerMaintenance);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEmployerMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _employerMaintenance.GetAllEmployerMaintenancesDatatable(sentParameters, searchValue);

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _employerMaintenance.AnyByCode(code, id);
    }
}
