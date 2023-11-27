using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template;
using AKDEMIC.SERVICE.Services.HelpDesk.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Implementations
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentsRepository _incidentRepository;

        public IncidentService(IIncidentsRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            return await _incidentRepository.GetIncidentsDatatable(sentParameters, searchValue,user);
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentTemplate>> GetIncidentByTechIdDatatable(DataTablesStructs.SentParameters sentParameters, string techId, string searchValue = null)
        {
            return await _incidentRepository.GetIncidentByTechIdDatatable(sentParameters, techId, searchValue);
        }


        public async Task<Incident> Get(Guid id)
        {
            return await _incidentRepository.Get(id);
        }

        public async Task Insert(Incident incident)
        {
            await _incidentRepository.Insert(incident);
        }

        public async Task Update(Incident incident)
        {
            await _incidentRepository.Update(incident);
        }
        public async Task DeleteById(Guid Id)
        {
            await _incidentRepository.DeleteById(Id);
        }
        public async Task<int> Count()
        {
            return await _incidentRepository.Count();
        }

        public async Task<IEnumerable<IncidentType>> GetTypes(Guid id)
        {
            return await _incidentRepository.GetTypes(id);
        }

        public async Task DeleteTypes(Guid id)
        {
            await _incidentRepository.DeleteTypes(id);
        }

        public async Task<List<Incident>> GetIncidentsByDates(DateTime start, DateTime end)
        {
            return await _incidentRepository.GetIncidentsByDates(start, end);
        }

        public async Task<TypeChartTemplate> GetTypesChart()
        {
            return await _incidentRepository.GetTypesChart();
        }

        public async Task<TechsChartTemplate> GetTechsChart()
        {
            return await _incidentRepository.GetTechsChart();
        }

        public async Task<MonthlyChartTemplate> GetMonthlyChart()
        {
            return await _incidentRepository.GetMonthlyChart();
        }
    }
}
