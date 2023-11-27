using AKDEMIC.ENTITIES.Models;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IPositionService
    {
        Task<Position> GetAsync(Guid id);
        Task DeleteAsync(Position position);
        Task InsertAsync(Position position);
        Task UpdateAsync(Position position);
        Task<object> GetAllAsModelA();
    }
}