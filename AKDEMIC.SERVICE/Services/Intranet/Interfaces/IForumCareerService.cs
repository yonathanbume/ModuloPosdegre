using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IForumCareerService
    {
        Task<IEnumerable<ForumCareer>> GetAllByForumId(Guid forumId);
        Task DeleteRange(IEnumerable<ForumCareer> forumCareers);
        void DeleteRangeWithOutSaving(IEnumerable<ForumCareer> forumCareers);        
    }
}
