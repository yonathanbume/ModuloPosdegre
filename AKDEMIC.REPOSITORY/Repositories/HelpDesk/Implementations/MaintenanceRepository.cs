using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations
{
    public class MaintenanceRepository : Repository<Maintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<MaintenanceTemplate, dynamic>> GetMaintenancesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Description);
                case "2":
                    return ((x) => x.UserReporting);
                case "3":
                    return ((x) => x.AssignedTechnician);
                case "4":
                    return ((x) => x.Date);
                default:
                    return ((x) => x.Code);
            }
        }

        private Func<MaintenanceTemplate, string[]> GetMaintenancesDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Code + "",
                x.Name + "",
                x.UserReporting+ "",
                x.AssignedTechnician+ "",
                x.Description+"",
                x.Date+"",
                x.Dependency+"",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<MaintenanceTemplate>> GetMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<MaintenanceTemplate, MaintenanceTemplate>> selectPredicate = null, Expression<Func<MaintenanceTemplate, dynamic>> orderByPredicate = null, Func<MaintenanceTemplate, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Maintenances
                .AsNoTracking();

            var result = query
                .Select(s => new MaintenanceTemplate
                {
                    Id = s.Id,
                    Code = s.CodeForUsers,
                    Description = s.Description,
                    StatusId = s.Status,
                    UserReporting = string.IsNullOrEmpty(s.UserReportingMaintenance.FullName) ? "No Especificado" : s.UserReportingMaintenance.FullName,
                    AssignedTechnician = string.IsNullOrEmpty(s.AssignedTechnician.FullName) ? "No Asignado" : s.AssignedTechnician.FullName,
                    Date = s.Date.ToString("dd/MM/yyyy"),
                    Dependency = s.Dependency.Name,
                }, searchValue)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC
        public async Task<DataTablesStructs.ReturnedData<MaintenanceTemplate>> GetMaintenancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetMaintenancesDatatable(sentParameters, null, GetMaintenancesDatatableOrderByPredicate(sentParameters), GetMaintenancesDatatableSearchValuePredicate(), searchValue);
        }
        #endregion
    }
}
