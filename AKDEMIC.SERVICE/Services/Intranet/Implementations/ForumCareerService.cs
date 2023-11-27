using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ForumCareerService : IForumCareerService
    {
        private readonly IForumCareerRepository _forumCareerRepository;

        public ForumCareerService(IForumCareerRepository forumCareerRepository)
        {
            _forumCareerRepository = forumCareerRepository;
        }

        public async Task DeleteRange(IEnumerable<ForumCareer> forumCareers)
        {
            await _forumCareerRepository.DeleteRange(forumCareers);
        }

        public void DeleteRangeWithOutSaving(IEnumerable<ForumCareer> forumCareers)
        {
            _forumCareerRepository.DeleteRangeWithOutSaving(forumCareers);
        }

        public async Task<IEnumerable<ForumCareer>> GetAllByForumId(Guid forumId)
            => await _forumCareerRepository.GetAllByForumId(forumId);   
    }
}
