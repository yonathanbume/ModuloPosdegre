using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IHardwareRepository:IRepository<Hardware>
    {
        Task<IEnumerable<Hardware>> GetHardwaresByComputer(Guid computerId);
        Task<DataTablesStructs.ReturnedData<object>> GetHardwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null);
    }
}
