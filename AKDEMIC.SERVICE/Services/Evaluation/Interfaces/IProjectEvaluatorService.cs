using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectEvaluatorService
    {
        Task<List<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid id);
        Task Insert(ProjectEvaluator evaluator);
        Task DeleteRange(List<ProjectEvaluator> projectEvaluators);
    }
}
