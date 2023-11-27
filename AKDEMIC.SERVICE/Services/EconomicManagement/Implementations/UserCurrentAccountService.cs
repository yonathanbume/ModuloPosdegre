using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class UserCurrentAccountService : IUserCurrentAccountService
    {
        private readonly IUserCurrentAccountRepository _userCurrentAccountRepository;
        public UserCurrentAccountService(IUserCurrentAccountRepository userCurrentAccountRepository)
        {
            _userCurrentAccountRepository = userCurrentAccountRepository;
        }

        public async Task DeleteById(Guid id) => await _userCurrentAccountRepository.DeleteById(id);

        public async Task DeleteRange(List<UserCurrentAccount> accounts) => await _userCurrentAccountRepository.DeleteRange(accounts);

        public async Task<List<UserCurrentAccount>> GetUserCurrentAccounts(string userId)
            => await _userCurrentAccountRepository.GetUserCurrentAccounts(userId);

        public async Task Insert(UserCurrentAccount userCurrentAccount)
            => await _userCurrentAccountRepository.Insert(userCurrentAccount);

        public async Task Update(UserCurrentAccount userCurrentAccount)
            => await _userCurrentAccountRepository.Update(userCurrentAccount);
    }
}
