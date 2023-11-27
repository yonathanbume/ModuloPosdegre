using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class LockedUserService : ILockedUserService
    {
        private readonly ILockedUserRepository _lockedUserRepository;

        public LockedUserService(ILockedUserRepository lockedUserRepository)
        {
            _lockedUserRepository = lockedUserRepository;
        }
        public async Task Lock(string userId, string text)
        {
            await _lockedUserRepository.Lock(userId, text);
        }
        public async Task Unlock(string userId, string text)
        {
            await _lockedUserRepository.Unlock(userId, text);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> Historic(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            return await _lockedUserRepository.Historic(sentParameters, userId);
        }
    }
}
