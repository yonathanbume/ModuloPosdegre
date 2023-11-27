using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
        }

        public async Task<Forum> Get(Guid id)
            => await _forumRepository.Get(id);

        public async Task Insert(Forum forum)
            => await _forumRepository.Insert(forum);

        public async Task Update(Forum forum)
            => await _forumRepository.Update(forum);

        public async Task<object> GetForums()
            => await _forumRepository.GetForums();

        public async Task<object> GetoForumById(Guid id, int careerCount)
            => await _forumRepository.GetoForumById(id, careerCount);

        public async Task UpdateForumCompleted(ForumTemplate model)
            => await _forumRepository.UpdateForumCompleted(model);

        public async Task DeleteForum(Guid id)
            => await _forumRepository.DeleteForum(id);

        public async Task<IEnumerable<Forum>> GetAllBySystem(int system)
        {
            return await _forumRepository.GetAllBySystem(system);
        }

        public async Task<Forum> GetWithIncludes(Guid id)
        {
            return await _forumRepository.GetWithIncludes(id);
        }

        public async Task DeleteById(Guid id)
        {
            await _forumRepository.DeleteById(id);
        }

        public async Task<object> GetCustomForumByCareer(Guid careerId, int sytem)
        {
            return await _forumRepository.GetCustomForumByCareer(careerId, sytem);
        }

        public async Task<List<Forum>> IndexForumJobExchange(string userId , int system, string search = null)
        {
            return await _forumRepository.IndexForumJobExchange(userId , system, search);
        }

        public async Task DeteleInformationForum(Guid ForumId)
        {
            await _forumRepository.DeteleInformationForum(ForumId);
        }

        public async Task<bool> HasTopics(Guid forumId)
            => await _forumRepository.HasTopics(forumId);

        public async Task<List<Forum>> GetForumBySystem(int system)
            => await _forumRepository.GetForumBySystem(system);
        public async Task<List<Forum>> GetForumBySystemToTutoring()
            => await _forumRepository.GetForumBySystemToTutoring();
    }
}
