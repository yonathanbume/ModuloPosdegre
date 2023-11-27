using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectRubricItemService
    {
        Task DeleteRange(IEnumerable<ProjectRubricItem> entities);
        Task<IEnumerable<ProjectRubricItem>> GetByProjectRubricId(Guid projectRubricId);
        Task<DataTablesStructs.ReturnedData<ProjectRubricItem>> GetProjectRubricItemByProjectRubricIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectRubricId, string search = null);
        Task<ProjectRubricItem> Get(Guid id);
        Task<bool> AnyByNameAndProjectRubricId(Guid projectRubricId, string name, Guid? ignoredId = null);
        Task<int> GetMaxTotalbyProjectRubricId(Guid projectRubricId, Guid? ignoredId = null);
        Task Insert(ProjectRubricItem entity);
        Task Update(ProjectRubricItem entity);
        Task Delete(ProjectRubricItem entity);
    }
}
