using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IComputerConditionFileRepository : IRepository<ComputerConditionFile>
    {
        Task<IEnumerable<ComputerConditionFile>> GetAllByComputerId(Guid computerId);
    }
}
