using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IUserCurrentAccountRepository : IRepository<UserCurrentAccount>
    {
        public Task<List<UserCurrentAccount>> GetUserCurrentAccounts(string userId);
    }
}
