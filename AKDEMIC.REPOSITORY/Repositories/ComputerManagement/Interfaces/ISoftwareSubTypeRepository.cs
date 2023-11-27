using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface ISoftwareSubTypeRepository : IRepository<SoftwareSubType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSoftwareSubTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid typeId, string searchValue = null);
    }
}
