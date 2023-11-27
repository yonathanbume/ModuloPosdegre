using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IUserCurrentAccountService
    {
        public Task<List<UserCurrentAccount>> GetUserCurrentAccounts(string userId);
        Task Insert(UserCurrentAccount userCurrentAccount);
        Task Update(UserCurrentAccount userCurrentAccount);
        Task DeleteById(Guid id);
        Task DeleteRange(List<UserCurrentAccount> accounts);
    }
}
