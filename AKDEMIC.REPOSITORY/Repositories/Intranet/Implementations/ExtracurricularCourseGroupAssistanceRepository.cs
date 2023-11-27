using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtracurricularCourseGroupAssistanceRepository : Repository<ExtracurricularCourseGroupAssistance>, IExtracurricularCourseGroupAssistanceRepository
    {
        public ExtracurricularCourseGroupAssistanceRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExtracurricularCourseGroupAssistance>> GetAllByGroup(Guid groupId)
            => await _context.ExtracurricularCourseGroupAssistances
                .Where(x => x.GroupId == groupId)
                .ToListAsync();
    }
}
