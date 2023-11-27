using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IForumRepository : IRepository<Forum>
    {
        Task<Forum> GetWithIncludes(Guid id);
        Task<object> GetCustomForumByCareer(Guid careerId, int system);
        Task<List<Forum>> IndexForumJobExchange(string userId, int system, string search = null);
        Task<object> GetForums();
        Task<object> GetoForumById(Guid id, int careerCount);
        Task UpdateForumCompleted(ForumTemplate model);
        Task DeleteForum(Guid id);
        Task<IEnumerable<Forum>> GetAllBySystem(int system);

        Task DeteleInformationForum(Guid ForumId);
        Task<bool> HasTopics(Guid forumId);
        Task<List<Forum>> GetForumBySystem(int system);
        Task<List<Forum>> GetForumBySystemToTutoring();
    }
}
