using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class InterestGroupUserRefreshTokenRepository : Repository<InterestGroupUserRefreshToken> , IInterestGroupUserRefreshTokenRepository
    {
        public InterestGroupUserRefreshTokenRepository(AkdemicContext context) : base(context) { }

        public async Task<InterestGroupUserRefreshToken> GetByUserId(string userId)
            => await _context.InterestGroupUserRefreshTokens.Where(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}
