using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IProjectRubricItemRepository : IRepository<ProjectRubricItem>
    {
        Task<IEnumerable<ProjectRubricItem>> GetByProjectRubricId(Guid projectRubricId);
        Task<DataTablesStructs.ReturnedData<ProjectRubricItem>> GetProjectRubricItemByProjectRubricIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectRubricId, string search = null);
        Task<bool> AnyByNameAndProjectRubricId(Guid projectRubricId, string name, Guid? ignoredId = null);
        Task<int> GetMaxTotalbyProjectRubricId(Guid projectRubricId, Guid? ignoredId = null);
    }
}
