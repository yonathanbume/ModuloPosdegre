using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces;
using AKDEMIC.SERVICE.Services.HelpDesk.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Implementations
{
    public class SolutionsService : ISolutionService
    {
        private readonly ISolutionsRepository _solutionRepository;

        public SolutionsService(ISolutionsRepository solutionsRepository)
        {
            _solutionRepository = solutionsRepository;
        }

        public async Task<int> Count()
        {
            return await _solutionRepository.Count();
        }

        public async Task DeleteIncidentSolution(Guid incidentId)
        {
            await _solutionRepository.Delete(incidentId);
        }

        public async Task<Solution> Get(Guid solutionId)
        {
            return await _solutionRepository.Get(solutionId);
        }

        public async Task<IncidentSolution> GetIncidentSolution(Guid incidentId)
        {
            return await _solutionRepository.GetIncidentSolution(incidentId);
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId)
        {
            return await _solutionRepository.GetIncidentSolutionsTable(sentParameters, incidenteId);
        }

        public async Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentsTable(DataTablesStructs.SentParameters sentParameters, Guid solutionId)
        {
            return await _solutionRepository.GetIncidentsTable(sentParameters, solutionId);
        }

        public async Task<DataTablesStructs.ReturnedData<SolutionTemplate>> GetSolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            return await _solutionRepository.GetSolutionsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<SolutionTemplate2>> GetSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId)
        {
            return await _solutionRepository.GetSolutionsTable(sentParameters, incidenteId);
        }

        public async Task Insert(Solution newSolution)
        {
            await _solutionRepository.Insert(newSolution);
        }

        public async Task InsertIncidentSolution(IncidentSolution newIncidentSolution)
        {
            await _solutionRepository.InsertIncidentSolution(newIncidentSolution);
        }

        public async Task InsertNewIncidentsSolution(Guid incidentId, List<Guid> idsSols)
        {
            await _solutionRepository.InsertNewIncidentsSolution(incidentId, idsSols);
        }

        public async Task Update(Solution editingSolution)
        {
            await _solutionRepository.Update(editingSolution);
        }
    }
}
