using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class ExtraTeachingLoadRepository : Repository<ExtraTeachingLoad>, IExtraTeachingLoadRepository
    {
        public ExtraTeachingLoadRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<ExtraTeachingLoad> GetExtraTeachingLoad(Guid termId, string teacherId)
            => await _context.ExtraTeachingLoads.Where(x => x.TeacherId == teacherId && x.TermId == termId).FirstOrDefaultAsync();
    }
}
