using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectItemScoreService : IProjectItemScoreService
    {
        private readonly IProjectItemScoreRepository _projectItemScoreRepository;

        public ProjectItemScoreService(IProjectItemScoreRepository projectItemScoreRepository)
        {
            _projectItemScoreRepository = projectItemScoreRepository;
        }

        public async Task<IEnumerable<ProjectItemScore>> GetAllByProjectAdvanceId(Guid projectAdvanceId)
            => await _projectItemScoreRepository.GetAllByProjectAdvanceId(projectAdvanceId);

        public async Task InsertRange(IEnumerable<ProjectItemScore> entities)
            => await _projectItemScoreRepository.InsertRange(entities);
    }
}
