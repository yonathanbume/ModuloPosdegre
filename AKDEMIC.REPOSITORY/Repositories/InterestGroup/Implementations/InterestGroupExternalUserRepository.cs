using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupExternalUserRepository : Repository<InterestGroupExternalUser>, IInterestGroupExternalUserRepository
    {
        public InterestGroupExternalUserRepository(AkdemicContext context) : base(context)
        {
        }
    }
}
