using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Interfaces
{
    public interface IMaintenanceService
    {
        Task<DataTablesStructs.ReturnedData<MaintenanceTemplate>> GetMaintenanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task DeleteById(Guid id);
        Task Insert(Maintenance maintenance);
        Task Update(Maintenance maintenance);
        Task<Maintenance> Get(Guid id);
        Task<int> Count();
    }
}
