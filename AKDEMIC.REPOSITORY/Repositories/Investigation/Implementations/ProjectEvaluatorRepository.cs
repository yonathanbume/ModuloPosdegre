using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class ProjectEvaluatorRepository : Repository<ProjectEvaluator>, IProjectEvaluatorRepository
    {
        public ProjectEvaluatorRepository(AkdemicContext context) : base(context) {  }

        public async Task<IEnumerable<ProjectEvaluator>> GetProjectEvaluatorsByProjectId(Guid projectId)
            => await _context.InvestigationProjectEvaluators.Where(x => x.ProjectId == projectId).ToArrayAsync();
    }
}
