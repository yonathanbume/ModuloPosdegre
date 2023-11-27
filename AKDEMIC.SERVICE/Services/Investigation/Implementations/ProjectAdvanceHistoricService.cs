using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectAdvanceHistoricService : IProjectAdvanceHistoricService
    {
        private readonly IProjectAdvanceHistoricRepository _projectAdvanceHistoricRepository;

        public ProjectAdvanceHistoricService(IProjectAdvanceHistoricRepository projectAdvanceHistoricRepository)
        {
            _projectAdvanceHistoricRepository = projectAdvanceHistoricRepository;
        }

        public async Task<ProjectAdvanceHistoric> Get(Guid id)
            => await _projectAdvanceHistoricRepository.Get(id);

        public async Task<IEnumerable<ProjectAdvanceHistoric>> GetAllbyProjectAdvanceId(Guid projectAdvanceId)
            => await _projectAdvanceHistoricRepository.GetAllbyProjectAdvanceId(projectAdvanceId);

        public async Task Insert(ProjectAdvanceHistoric entity)
            => await _projectAdvanceHistoricRepository.Insert(entity);

        public async Task Update(ProjectAdvanceHistoric entity)
            => await _projectAdvanceHistoricRepository.Update(entity);
    }
}
