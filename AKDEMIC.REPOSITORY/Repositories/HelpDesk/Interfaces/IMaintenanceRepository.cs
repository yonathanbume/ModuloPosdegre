using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces
{
    public interface IMaintenanceRepository
    {
        Task<DataTablesStructs.ReturnedData<MaintenanceTemplate>> GetMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<Maintenance> Get(Guid id);
        Task Insert(Maintenance maintenance);
        Task Update(Maintenance maintenance);
        Task DeleteById(Guid id);
        Task<int> Count();
    }
}
