using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> GetPostByPostCitedId(Guid id);
        Task<Post> GetByTopicAndLevel(Guid topicId, int level);
        Task<Post> GetWithIncludesByTopic(Guid topicId);
        Task<Post> GetWithIncludes(Guid id);
        Task<IEnumerable<Post>> GetPostsByTopicIdAndForumId(Guid topicId, Guid forumId);
        Task<IEnumerable<Post>> GetPostsByTopicIdAndForumIdNotDeleted(Guid topicId, Guid forumId);
        Task<Post> GetPostByTopicId(Guid topicId);
    }
}
