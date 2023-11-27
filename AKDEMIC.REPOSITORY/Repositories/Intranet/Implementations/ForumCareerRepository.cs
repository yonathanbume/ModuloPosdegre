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
    public class ForumCareerRepository : Repository<ForumCareer>, IForumCareerRepository
    {
        public ForumCareerRepository(AkdemicContext context):base(context) { }

        public void DeleteRangeWithOutSaving(IEnumerable<ForumCareer> forumCareers)
        {
            _context.Set<ForumCareer>().RemoveRange(forumCareers);
        }

        public async Task<IEnumerable<ForumCareer>> GetAllByForumId(Guid forumId)
            => await _context.ForumCareers.Where(x => x.ForumId == forumId).ToArrayAsync();

    }
}
