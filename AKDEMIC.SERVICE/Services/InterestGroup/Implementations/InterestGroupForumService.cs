using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupForum;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupForumService : IInterestGroupForumService
    {
        private readonly IInterestGroupForumRepository _interestGroupForumRepository;

        public InterestGroupForumService(IInterestGroupForumRepository interestGroupForumRepository)
        {
            _interestGroupForumRepository = interestGroupForumRepository;
        }

        public async Task Delete(InterestGroupForum interestGroupForum)
            => await _interestGroupForumRepository.Delete(interestGroupForum);

        public async Task<InterestGroupForum> GetByForumId(Guid forumId)
            => await _interestGroupForumRepository.GetByForumId(forumId);

        public async Task<IEnumerable<Forum>> GetForumByRoleAndUserId(string role, string userId)
            => await _interestGroupForumRepository.GetForumByRoleAndUserId(role, userId);

        public async Task<IEnumerable<ForumReportTemplate>> GetForumReportData(Guid interestGroupId)
        {
            return await _interestGroupForumRepository.GetForumReportData(interestGroupId);
        }

        public async Task<DataTablesStructs.ReturnedData<ForumReportTemplate>> GetForumReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId, string searchValue = null)
            => await _interestGroupForumRepository.GetForumReportDatatable(sentParameters, interestGroupId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<InterestGroupForum>> GetInterestGroupForumDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicProgramId = null, string searchValue = null, string userAdminId = null)
            => await _interestGroupForumRepository.GetInterestGroupForumDatatable(sentParameters, academicProgramId, searchValue, userAdminId);

        public async Task<IEnumerable<InterestGroupForum>> GetInterestGroupForumsByInterestGroupId(Guid interestGroupId)
            => await _interestGroupForumRepository.GetInterestGroupForumsByInterestGroupId(interestGroupId);

        public async Task Insert(InterestGroupForum entity)
            => await _interestGroupForumRepository.Insert(entity);
    }
}
