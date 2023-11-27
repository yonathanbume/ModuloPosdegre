using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces
{
    public interface IInterestGroupUserRefreshTokenRepository : IRepository<InterestGroupUserRefreshToken>
    {
        Task<InterestGroupUserRefreshToken> GetByUserId(string userId);
    }
}
