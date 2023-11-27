using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Interfaces
{
    public interface IIncidentService
    {
        Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentByTechIdDatatable(DataTablesStructs.SentParameters sentParameters, string techId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(Incident incident);
        Task Update(Incident incident);
        Task<Incident> Get(Guid id);
        Task<int> Count();
        Task<IEnumerable<IncidentType>> GetTypes(Guid id);
        Task DeleteTypes(Guid id);
        Task<List<Incident>> GetIncidentsByDates(DateTime start, DateTime end);
        Task<TypeChartTemplate> GetTypesChart();
        Task<TechsChartTemplate> GetTechsChart();
        Task<MonthlyChartTemplate> GetMonthlyChart();
    }
}
