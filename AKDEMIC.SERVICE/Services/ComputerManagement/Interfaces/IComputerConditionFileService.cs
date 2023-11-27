using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IComputerConditionFileService
    {
        Task InsertRange(IEnumerable<ComputerConditionFile> entites);
        Task Insert(ComputerConditionFile entity);
        Task Delete(ComputerConditionFile entity);
        Task<ComputerConditionFile> Get(Guid id);
        Task<IEnumerable<ComputerConditionFile>> GetAllByComputerId(Guid computerId);
    }
}
