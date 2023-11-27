using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using AKDEMIC.SERVICE.Services.HelpDesk.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Implementations
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _MaintenanceRepository;

        public MaintenanceService(IMaintenanceRepository MaintenanceRepository)
        {
            _MaintenanceRepository = MaintenanceRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<MaintenanceTemplate>> GetMaintenanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _MaintenanceRepository.GetMaintenancesDatatable(sentParameters, searchValue);
        }

        public async Task<Maintenance> Get(Guid id)
        {
            return await _MaintenanceRepository.Get(id);
        }

        public async Task Insert(Maintenance Maintenance)
        {
            await _MaintenanceRepository.Insert(Maintenance);
        }

        public async Task Update(Maintenance Maintenance)
        {
            await _MaintenanceRepository.Update(Maintenance);
        }
        public async Task DeleteById(Guid Id)
        {
            await _MaintenanceRepository.DeleteById(Id);
        }
        public async Task<int> Count()
        {
            return await _MaintenanceRepository.Count();
        }
    }
}
