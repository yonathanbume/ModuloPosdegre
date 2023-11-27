using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task Delete(Topic topic)
            => await _topicRepository.Delete(topic);

        public async Task<Topic> Get(Guid id)
            => await _topicRepository.Get(id);

        public async Task<IEnumerable<Topic>> GetTopicsByForumId(Guid forumId)
            => await _topicRepository.GetTopicsByForumId(forumId);

        public async Task Insert(Topic topic)
            => await _topicRepository.Insert(topic);

        public async Task<object> GetTopicsHome()
            => await _topicRepository.GetTopicsHome();

        public async Task<List<Topic>> GetCustomAllWithIncludesByForum(Guid forumId, string search = null)
        {
            return await _topicRepository.GetCustomAllWithIncludesByForum(forumId, search);
        }

        public async Task<bool> HasPosts(Guid id)
        {
            return await _topicRepository.HasPosts(id);
        }
    }
}
