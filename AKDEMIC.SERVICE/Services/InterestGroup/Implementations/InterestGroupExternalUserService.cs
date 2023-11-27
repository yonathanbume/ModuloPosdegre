using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupExternalUserService : IInterestGroupExternalUserService
    {
        private IInterestGroupExternalUserRepository _interestGroupExternalUserRepository;
        public InterestGroupExternalUserService(IInterestGroupExternalUserRepository interestGroupExternalUserRepository)
        {
            _interestGroupExternalUserRepository = interestGroupExternalUserRepository;
        }

        public async Task DeleteById(string userId)
        {
            await _interestGroupExternalUserRepository.DeleteById(userId);
        }

        public async Task<InterestGroupExternalUser> Get(string id)
        {
            return await _interestGroupExternalUserRepository.Get(id);
        }

        public async Task Insert(InterestGroupExternalUser interestGroupExternalUser)
        {
            await _interestGroupExternalUserRepository.Insert(interestGroupExternalUser);
        }

        public async Task Update(InterestGroupExternalUser entity)
            => await _interestGroupExternalUserRepository.Update(entity);
    }
}
