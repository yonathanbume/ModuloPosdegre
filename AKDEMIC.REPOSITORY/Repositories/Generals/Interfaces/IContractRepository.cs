using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<object> GetAllAsModelA();
    }
}