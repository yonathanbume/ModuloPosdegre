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
    public class ProjectAdvanceHistoricRepository : Repository<ProjectAdvanceHistoric>, IProjectAdvanceHistoricRepository
    {
        public ProjectAdvanceHistoricRepository(AkdemicContext context) :base(context) { }

        public async Task<IEnumerable<ProjectAdvanceHistoric>> GetAllbyProjectAdvanceId(Guid projectAdvanceId)
            => await _context.InvestigationProjectAdvanceHistorics.Where(X => X.ProjectAdvanceId == projectAdvanceId).ToArrayAsync();

        public override async Task<ProjectAdvanceHistoric> Get(Guid id)
            => await _context.InvestigationProjectAdvanceHistorics.Include(x => x.ProjectAdvance).Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}
