using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class ProjectEvaluatorService : IProjectEvaluatorService
    {
        private readonly IProjectEvaluatorRepository _projectEvaluatorRepository;

        public ProjectEvaluatorService(IProjectEvaluatorRepository projectEvaluatorRepository)
        {
            _projectEvaluatorRepository = projectEvaluatorRepository;
        }

        public async Task<IEnumerable<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid projectId)
            => await _projectEvaluatorRepository.GetProjectEvaluatorsByProjectId(projectId);

        public async Task Insert(ProjectEvaluator projectEvaluator)
            => await _projectEvaluatorRepository.Insert(projectEvaluator);

        public async Task DeleteRange(IEnumerable<ProjectEvaluator> entities)
            => await _projectEvaluatorRepository.DeleteRange(entities);
    }
}
