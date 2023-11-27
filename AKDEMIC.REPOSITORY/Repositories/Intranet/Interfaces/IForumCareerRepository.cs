using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IForumCareerRepository : IRepository<ForumCareer>
    {
        Task<IEnumerable<ForumCareer>> GetAllByForumId(Guid forumId);
        void DeleteRangeWithOutSaving(IEnumerable<ForumCareer> forumCareers);
    }
}
