using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
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
        Task<ProjectAdvance> GetByProjectId(Guid projectId, bool? isFinal = null);
    }
}
