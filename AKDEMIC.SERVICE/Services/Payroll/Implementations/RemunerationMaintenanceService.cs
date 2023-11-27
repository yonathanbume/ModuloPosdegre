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
    public class RemunerationMaintenanceService : IRemunerationMaintenanceService
    {
        private readonly IRemunerationMaintenanceRepository _remunerationMaintenance;

        public RemunerationMaintenanceService(IRemunerationMaintenanceRepository remunerationMaintenance)
        {
            _remunerationMaintenance = remunerationMaintenance;
        }

        public async Task Delete(RemunerationMaintenance remunerationMaintenance)
    => await _remunerationMaintenance.Delete(remunerationMaintenance);

        public async Task<RemunerationMaintenance> Get(Guid id)
            => await _remunerationMaintenance.Get(id);

        public async Task<IEnumerable<RemunerationMaintenance>> GetAll()
            => await _remunerationMaintenance.GetAll();

        public async Task Insert(RemunerationMaintenance remunerationMaintenance)
    => await _remunerationMaintenance.Insert(remunerationMaintenance);

        public async Task Update(RemunerationMaintenance remunerationMaintenance)
            => await _remunerationMaintenance.Update(remunerationMaintenance);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllRemunerationMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _remunerationMaintenance.GetAllRemunerationMaintenancesDatatable(sentParameters, searchValue);
    }
}
