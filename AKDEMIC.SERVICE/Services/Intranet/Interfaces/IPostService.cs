using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IPostService
    {
        Task<Post> GetPostByPostCitedId(Guid id);
        Task<Post> GetWithIncludes(Guid id);
        Task<Post> GetWithIncludesByTopic(Guid topicId);
        Task<Post> GetByTopicAndLevel(Guid topicId , int level);
        Task<IEnumerable<Post>> GetPostsByTopicIdAndForumIdNotDeleted(Guid topicId, Guid forumId);
        Task<IEnumerable<Post>> GetPostsByTopicIdAndForumId(Guid topicId, Guid forumId);
        Task Insert(Post topic);
        Task<Post> GetPostByTopicId(Guid topicId);
        Task<Post> Get(Guid id);
        Task Delete(Post post);
    }
}
