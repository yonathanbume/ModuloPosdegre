using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;

namespace AKDEMIC.SERVICE.Services.Investigation.Interfaces
{
    public interface ILockedUserService
    {
        Task Lock(string userId, string text);
        Task Unlock(string userId, string text);
        Task<DataTablesStructs.ReturnedData<object>> Historic(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}