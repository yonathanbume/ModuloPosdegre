using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface ILockedUserRepository : IRepository<LockedUser>
    {
        Task Lock(string userId, string text);
        Task Unlock(string userId, string text);
        Task<DataTablesStructs.ReturnedData<object>> Historic(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
