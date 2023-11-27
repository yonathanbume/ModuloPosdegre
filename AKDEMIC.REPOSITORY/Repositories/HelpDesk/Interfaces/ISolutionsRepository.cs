using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Interfaces
{
    public interface ISolutionsRepository
    {
        Task<int> Count();
        Task Delete(Guid incidentId);
        Task<Solution> Get(Guid solutionId);
        Task<IncidentSolution> GetIncidentSolution(Guid incidentId);
        Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId);
        Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentsTable(DataTablesStructs.SentParameters sentParameters, Guid solutionId);
        Task<DataTablesStructs.ReturnedData<SolutionTemplate>> GetSolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<SolutionTemplate2>> GetSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId);
        Task Insert(Solution newSolution);
        Task InsertIncidentSolution(IncidentSolution newIncidentSolution);
        Task InsertNewIncidentsSolution(Guid incidentId, List<Guid> idsSols);
        Task Update(Solution editingSolution);
    }
}
