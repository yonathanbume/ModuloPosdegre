using AKDEMIC.ENTITIES.Models.Generals;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IDeanService
    {
        Task<Dean> Get(string id);
        Task DeleteById(string deanId);
        Task Insert(Dean dean);
    }
}
