using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IForumService
    {
        Task<Forum> Get(Guid id);
        Task<Forum> GetWithIncludes(Guid id);
        Task<object> GetCustomForumByCareer(Guid careerId , int system);
        Task<List<Forum>> IndexForumJobExchange(string userId, int system, string search = null);
        Task<IEnumerable<Forum>> GetAllBySystem(int system);
        Task Insert(Forum forum);
        Task Update(Forum forum);
        Task<object> GetForums();
        Task<object> GetoForumById(Guid id, int careerCount);
        Task UpdateForumCompleted(ForumTemplate model);
        Task DeleteForum(Guid id);
        Task DeleteById(Guid id);
        Task DeteleInformationForum(Guid ForumId);

        Task<bool> HasTopics(Guid forumId);
        Task<List<Forum>> GetForumBySystem(int system);
        Task<List<Forum>> GetForumBySystemToTutoring();
    }
}
