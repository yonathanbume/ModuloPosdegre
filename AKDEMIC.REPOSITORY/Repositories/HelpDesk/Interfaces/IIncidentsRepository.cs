using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces
{
    public interface IIncidentsRepository : IRepository<Incident>
    {
        Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null,ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentByTechIdDatatable(DataTablesStructs.SentParameters sentParameters, string techId, string searchValue = null);
        Task<IEnumerable<IncidentType>> GetTypes(Guid id);
        Task DeleteTypes(Guid id);
        Task<List<Incident>> GetIncidentsByDates(DateTime start, DateTime end);
        Task<TypeChartTemplate> GetTypesChart();
        Task<TechsChartTemplate> GetTechsChart();
        Task<MonthlyChartTemplate> GetMonthlyChart();
    }
}
