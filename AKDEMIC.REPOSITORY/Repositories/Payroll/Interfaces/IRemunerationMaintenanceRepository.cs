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
    public interface IRemunerationMaintenanceRepository : IRepository<RemunerationMaintenance>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllRemunerationMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
