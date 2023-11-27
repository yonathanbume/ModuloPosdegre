using AKDEMIC.ENTITIES.Models;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IContractService
    {
        Task<Contract> GetAsync(Guid id);
        Task InsertAsync(Contract contract);
        Task UpdateAsync(Contract contract);
        Task DeleteAsync(Contract contract);
        Task<object> GetAllAsModelA();
    }
}