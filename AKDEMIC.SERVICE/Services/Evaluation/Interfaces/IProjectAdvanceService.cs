using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectAdvanceService
    {
        Task<bool> AnyProjectAdvanceByName(string name, Guid? id, Guid projectId);
        Task<int> Count(Guid projectId);
        Task<ProjectAdvance> Get(Guid id);
        Task<object> GetProjectAdvance(Guid id);
        Task<IEnumerable<object>> GetProjectAdvances(Guid projectId);
        Task<List<ProjectAdvanceTemplate>> GetProjectAdvancesTemplate(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectAdvancesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(ProjectAdvance advance);
        Task Update(ProjectAdvance advance);
        Task<Project> GetProjectByProjectAdvanceId(Guid id);
        Task<ProjectAdvance> IsFinal(Guid projectId);
        Task<ProjectAdvance> GetWithProjectItemScores(Guid advanceId);
        Task<string> GetAdvanceHistoryUrl(Guid id);
    }
}
