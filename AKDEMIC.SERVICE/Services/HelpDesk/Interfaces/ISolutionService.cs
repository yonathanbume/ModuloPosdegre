using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.REPOSITORY.Repositories.HelpDesk.Implementations;

namespace AKDEMIC.SERVICE.Services.HelpDesk.Interfaces
{
    public interface ISolutionService
    {
        //solutions
        Task<int> Count();
        Task Insert(Solution newSolution);
        Task Update(Solution editingSolution);
        Task<Solution> Get(Guid solutionId);
        Task<DataTablesStructs.ReturnedData<SolutionTemplate>> GetSolutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        //incident solutions
        Task DeleteIncidentSolution(Guid incidentId);
        Task<IncidentSolution> GetIncidentSolution(Guid incidentId);
        Task InsertIncidentSolution(IncidentSolution newIncidentSolution);
        Task InsertNewIncidentsSolution(Guid incidentId, List<Guid> idsSols);
        //incident solutions tables
        Task<DataTablesStructs.ReturnedData<SolutionTemplate2>> GetSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId);
        Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentsTable(DataTablesStructs.SentParameters sentParameters, Guid solutionId);
        Task<DataTablesStructs.ReturnedData<IncidentSolutionTemplate>> GetIncidentSolutionsTable(DataTablesStructs.SentParameters sentParameters, Guid incidenteId);        
    }
}
