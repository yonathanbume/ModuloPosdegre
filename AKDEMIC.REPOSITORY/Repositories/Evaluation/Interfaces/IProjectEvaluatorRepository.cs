using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectEvaluatorRepository:IRepository<ProjectEvaluator>
    {
        Task<List<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid id);
    }
}
