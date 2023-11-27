using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectAdvanceRepository : IRepository<ProjectAdvance>
    {
        Task<int> Count(Guid projectId);
        Task<object> GetProjectAdvance(Guid id);
        Task<bool> AnyProjectAdvanceByName(string name, Guid? id, Guid projectId);
        Task<IEnumerable<object>> GetProjectAdvances(Guid projectId);
        Task<List<ProjectAdvanceTemplate>> GetProjectAdvancesTemplate(Guid projectId);
        Task<DataTablesStructs.ReturnedData<object>> GetProjectAdvancesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid projectId, string searchValue = null);
        Task<ProjectAdvance> GetByProjectId(Guid projectId, bool? isFinal = null);
    }
}
