using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectRubricItemService : IProjectRubricItemService
    {
        private readonly IProjectRubricItemRepository _projectRubricItemRepository;

        public ProjectRubricItemService(IProjectRubricItemRepository projectRubricItemRepository)
        {
            _projectRubricItemRepository = projectRubricItemRepository;
        }

        public async Task<bool> AnyByNameAndProjectRubricId(Guid projectRubricId, string name, Guid? ignoredId = null)
            => await _projectRubricItemRepository.AnyByNameAndProjectRubricId(projectRubricId, name, ignoredId);

        public async Task Delete(ProjectRubricItem entity)
            => await _projectRubricItemRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<ProjectRubricItem> entities)
            => await _projectRubricItemRepository.DeleteRange(entities);

        public async Task<ProjectRubricItem> Get(Guid id)
            => await _projectRubricItemRepository.Get(id);

        public async Task<IEnumerable<ProjectRubricItem>> GetByProjectRubricId(Guid projectRubricId)
            => await _projectRubricItemRepository.GetByProjectRubricId(projectRubricId);

        public async Task<int> GetMaxTotalbyProjectRubricId(Guid projectRubricId, Guid? ignoredId = null)
            => await _projectRubricItemRepository.GetMaxTotalbyProjectRubricId(projectRubricId,ignoredId);

        public async Task<DataTablesStructs.ReturnedData<ProjectRubricItem>> GetProjectRubricItemByProjectRubricIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectRubricId, string search = null)
            => await _projectRubricItemRepository.GetProjectRubricItemByProjectRubricIdDatatable(sentParameters, projectRubricId, search);

        public async Task Insert(ProjectRubricItem entity)
            => await _projectRubricItemRepository.Insert(entity);

        public async Task Update(ProjectRubricItem entity)
            => await _projectRubricItemRepository.Update(entity);
    }
}
