using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseSyllabusWeekRepository : Repository<CourseSyllabusWeek> , ICourseSyllabusWeekRepository
    {
        public CourseSyllabusWeekRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<CourseSyllabusWeek>> GetAllByCourseSyllabusId(Guid courseSyllabusId)
            => await _context.CourseSyllabusWeeks.Where(x => x.CourseSyllabusId == courseSyllabusId).ToArrayAsync();
    }
}
