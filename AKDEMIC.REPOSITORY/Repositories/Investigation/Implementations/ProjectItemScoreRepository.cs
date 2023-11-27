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
    public class ProjectItemScoreRepository : Repository<ProjectItemScore>, IProjectItemScoreRepository
    {
        public ProjectItemScoreRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ProjectItemScore>> GetAllByProjectAdvanceId(Guid projectAdvanceId)
            => await _context.InvestigationProjectItemScores.Include(x=>x.ProjectRubricItem).Where(x => x.ProjectAdvanceId == projectAdvanceId).ToArrayAsync();
    }
}
