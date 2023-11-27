using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Structs;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectRubricService : IProjectRubricService
    {
        private readonly IProjectRubricRepository _projectRubricRepository;

        public ProjectRubricService(IProjectRubricRepository projectRubricRepository)
        {
            _projectRubricRepository = projectRubricRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _projectRubricRepository.AnyByName(name,ignoredId);

        public async Task Delete(ProjectRubric entity)
            => await _projectRubricRepository.Delete(entity);

        public async Task<ProjectRubric> Get(Guid id)
            => await _projectRubricRepository.Get(id);

        public async Task<ProjectRubric> GetByType(byte type)
            => await _projectRubricRepository.GetByType(type);

        public async Task<DataTablesStructs.ReturnedData<ProjectRubric>> GetProjectRubricDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _projectRubricRepository.GetProjectRubricDatatable(sentParameters, search);

        public async Task<IEnumerable<ProjectRubric>> GetProjectRubricsByType(byte type, bool? status = null)
            => await _projectRubricRepository.GetProjectRubricsByType(type, status);

        public async Task<IEnumerable<Select2Structs.Result>> GetProjectRubricsSelect2ClientSide(byte? type = null)
            => await _projectRubricRepository.GetProjectRubricsSelect2ClientSide(type);

        public async Task Insert(ProjectRubric projectRubric)
            => await _projectRubricRepository.Insert(projectRubric);

        public async Task Update(ProjectRubric entity)
            => await _projectRubricRepository.Update(entity);

        public async Task UpdateRange(IEnumerable<ProjectRubric> entites)
            => await _projectRubricRepository.UpdateRange(entites);
    }
}
