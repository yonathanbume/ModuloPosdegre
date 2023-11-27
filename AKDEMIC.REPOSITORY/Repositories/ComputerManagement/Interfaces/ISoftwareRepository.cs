using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface ISoftwareRepository:IRepository<Software>
    {
        Task<IEnumerable<Software>> GetSoftwaresByComputer(Guid computerId);
        Task<DataTablesStructs.ReturnedData<object>> GetSoftwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null);
    }
}
