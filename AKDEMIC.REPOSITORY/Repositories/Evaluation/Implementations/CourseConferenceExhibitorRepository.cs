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
    public class CourseConferenceExhibitorRepository : Repository<CourseConferenceExhibitor>, ICourseConferenceExhibitorRepository
    {
        public CourseConferenceExhibitorRepository(AkdemicContext context):base(context) { }

        public async Task<IEnumerable<CourseConferenceExhibitor>> GetAllByCourseConferenceIdAsync(Guid courseCoferenceId)
            => await _context.CourseConferenceExhibitors.Where(x => x.CourseConferenceId == courseCoferenceId).ToArrayAsync();
    }
}
