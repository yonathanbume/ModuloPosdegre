using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class ProjectEvaluatorRepository : Repository<ProjectEvaluator>, IProjectEvaluatorRepository
    {
        public ProjectEvaluatorRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid id)
        {
            return await _context.EvaluationProjectEvaluators.Where(x => x.ProjectId == id).ToListAsync();
        }
    }
}
