using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.SERVICE.Services.InterestGroup.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Implementations
{
    public class InterestGroupUserRefreshTokenService : IInterestGroupUserRefreshTokenService
    {
        private readonly IInterestGroupUserRefreshTokenRepository _interestGroupUserRefreshTokenRepository;

        public InterestGroupUserRefreshTokenService(IInterestGroupUserRefreshTokenRepository interestGroupUserRefreshTokenRepository)
        {
            _interestGroupUserRefreshTokenRepository = interestGroupUserRefreshTokenRepository;
        }

        public async Task<InterestGroupUserRefreshToken> GetByUserId(string userId)
            => await _interestGroupUserRefreshTokenRepository.GetByUserId(userId);

        public async Task Insert(InterestGroupUserRefreshToken entity)
            => await _interestGroupUserRefreshTokenRepository.Insert(entity);

        public async Task Update(InterestGroupUserRefreshToken entity)
            => await _interestGroupUserRefreshTokenRepository.Update(entity);
    }
}
