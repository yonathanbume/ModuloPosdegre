using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<Topic>> GetTopicsByForumId(Guid forumId);
        Task<Topic> Get(Guid id);
        Task Insert(Topic topic);
        Task Delete(Topic topic);
        Task<object> GetTopicsHome();
        Task<List<Topic>> GetCustomAllWithIncludesByForum(Guid forumId, string search = null);
        Task<bool> HasPosts(Guid id);
    }
}
