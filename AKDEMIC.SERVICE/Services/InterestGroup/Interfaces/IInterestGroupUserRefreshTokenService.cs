using AKDEMIC.ENTITIES.Models.InterestGroup;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IInterestGroupUserRefreshTokenService
    {
        Task<InterestGroupUserRefreshToken> GetByUserId(string userId);
        Task Insert(InterestGroupUserRefreshToken entity);
        Task Update(InterestGroupUserRefreshToken entity);
    }
}
