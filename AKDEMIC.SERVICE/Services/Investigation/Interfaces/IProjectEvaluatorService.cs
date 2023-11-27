using AKDEMIC.ENTITIES.Models.Investigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface IProjectEvaluatorService
    {
        Task<IEnumerable<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid projectId);
        Task Insert(ProjectEvaluator projectEvaluator);
        Task DeleteRange(IEnumerable<ProjectEvaluator> entities);
    }
}
