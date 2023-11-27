using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        Task<object> GetAllAsModelA();
    }
}