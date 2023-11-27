using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IRemunerationMaintenanceService
    {
        Task<RemunerationMaintenance> Get(Guid id);

        Task<IEnumerable<RemunerationMaintenance>> GetAll();

        Task Insert(RemunerationMaintenance remunerationMaintenance);
        Task Update(RemunerationMaintenance remunerationMaintenance);
        Task Delete(RemunerationMaintenance remunerationMaintenance);

        Task<DataTablesStructs.ReturnedData<object>> GetAllRemunerationMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
