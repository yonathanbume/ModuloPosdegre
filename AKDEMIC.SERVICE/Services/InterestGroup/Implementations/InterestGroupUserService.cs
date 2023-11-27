using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupUsersTemplate;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupUserService : IInterestGroupUserService
    {
        private readonly IInterestGroupUserRepository _interestGroupUserRepository;

        public InterestGroupUserService(IInterestGroupUserRepository interestGroupUserRepository)
        {
            _interestGroupUserRepository = interestGroupUserRepository;
        }

        public async Task<IEnumerable<InterestGroupUser>> GetInterestGroupUsersByInterestGroupId(Guid interestGroupId)
            => await _interestGroupUserRepository.GetInterestGroupUsersByInterestGroupId(interestGroupId);

        public async Task Insert(InterestGroupUser entity)
            => await _interestGroupUserRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<InterestGroupUser> entites)
            => await _interestGroupUserRepository.InsertRange(entites);

        public async Task Delete(InterestGroupUser entity)
            => await _interestGroupUserRepository.Delete(entity);

        public async Task<InterestGroupUser> GetInterestGroupUserByUserId(string userId)
            => await _interestGroupUserRepository.GetInterestGroupUserByUserId(userId);

        public async Task DeleteRange(IEnumerable<InterestGroupUser> entities)
            => await _interestGroupUserRepository.DeleteRange(entities);

        public async Task<IEnumerable<ApplicationUser>> GetUsersByInterestGroupId(Guid interestGroupId)
            => await _interestGroupUserRepository.GetUsersByInterestGroupId(interestGroupId);

        public async Task<IEnumerable<InterestGroupUserTemplate>> GetInterestGroupUsersBySurveyId(Guid id)
        {
            return await _interestGroupUserRepository.GetInterestGroupUsersBySurveyId(id);
        }

        public async Task<DataTablesStructs.ReturnedData<InterestGroupUser>> GetInterestGroupUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid? interestGroupId = null, string searchValue = null)
            => await _interestGroupUserRepository.GetInterestGroupUserDatatable(sentParameters, interestGroupId, searchValue);
    }
}
