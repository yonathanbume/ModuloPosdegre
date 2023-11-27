using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Delete(Post post)
            => await _postRepository.Delete(post);

        public async Task<Post> Get(Guid id)
            => await _postRepository.Get(id);

        public async Task<Post> GetByTopicAndLevel(Guid topicId, int level)
        {
            return await _postRepository.GetByTopicAndLevel(topicId, level);
        }

        public async Task<Post> GetPostByPostCitedId(Guid id)
            => await _postRepository.GetPostByPostCitedId(id);

        public async Task<Post> GetPostByTopicId(Guid topicId)
            => await _postRepository.GetPostByTopicId(topicId);

        public async Task<IEnumerable<Post>> GetPostsByTopicIdAndForumId(Guid topicId, Guid forumId)
            => await _postRepository.GetPostsByTopicIdAndForumId(topicId, forumId);

        public async Task<IEnumerable<Post>> GetPostsByTopicIdAndForumIdNotDeleted(Guid topicId, Guid forumId)
        {
            return await _postRepository.GetPostsByTopicIdAndForumIdNotDeleted(topicId,forumId);
        }

        public async Task<Post> GetWithIncludes(Guid id)
        {
            return await _postRepository.GetWithIncludes(id);
        }

        public async Task<Post> GetWithIncludesByTopic(Guid topicId)
        {
            return await _postRepository.GetWithIncludesByTopic(topicId);
        }

        public async Task Insert(Post post)
            => await _postRepository.Insert(post);
    }
}
