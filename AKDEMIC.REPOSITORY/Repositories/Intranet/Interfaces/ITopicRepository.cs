using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<IEnumerable<Topic>> GetTopicsByForumId(Guid forumId);
        Task<object> GetTopicsHome();
        Task<List<Topic>> GetCustomAllWithIncludesByForum(Guid forumId, string search = null);
        Task<bool> HasPosts(Guid id);
    }
}
