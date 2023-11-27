using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class UserCurrentAccountRepository : Repository<UserCurrentAccount>, IUserCurrentAccountRepository
    {
        public UserCurrentAccountRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<UserCurrentAccount>> GetUserCurrentAccounts(string userId)
        {
            var accounts = await _context.UserCurrentAccounts
                .Where(x => x.UserId == userId)
                .Include(x => x.CurrentAccount)
                .ToListAsync();

            return accounts;
        }
    }
}
