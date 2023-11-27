using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.InterestGroup;

namespace AKDEMIC.SERVICE.Services.InterestGroup.Interfaces
{
    public interface IInterestGroupExternalUserService
    {
        Task Insert(InterestGroupExternalUser interestGroupExternalUser);
        Task<InterestGroupExternalUser> Get(string id);
        Task DeleteById(string userId);
        Task Update(InterestGroupExternalUser entity);
    }
}
