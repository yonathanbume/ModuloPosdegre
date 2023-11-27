using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class ProjectEvaluatorService : IProjectEvaluatorService
    {
        private readonly IProjectEvaluatorRepository _projectEvaluatorRepository;
        public ProjectEvaluatorService(IProjectEvaluatorRepository projectEvaluatorRepository)
        {
            _projectEvaluatorRepository = projectEvaluatorRepository;
        }

        public async Task DeleteRange(List<ProjectEvaluator> projectEvaluators)
        {
            await _projectEvaluatorRepository.DeleteRange(projectEvaluators);
        }

        public async Task<List<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid id)
        {
            return await _projectEvaluatorRepository.GetProjectEvaluatorsByProjectId(id);
        }

        public async Task Insert(ProjectEvaluator evaluator)
        {
            await _projectEvaluatorRepository.Insert(evaluator);
        }
    }
}
