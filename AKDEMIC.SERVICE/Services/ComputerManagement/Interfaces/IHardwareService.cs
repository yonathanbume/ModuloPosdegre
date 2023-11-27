using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IHardwareService
    {
        //Generales
        Task Insert(Hardware hardware);
        Task Update(Hardware hardware);
        Task Delete(Hardware hardware);
        Task DeleteById(Guid id);
        Task<Hardware> Get(Guid id);
        Task<IEnumerable<Hardware>> GetAll();

        Task<IEnumerable<Hardware>> GetHardwaresByComputer(Guid computerId);
        Task<DataTablesStructs.ReturnedData<object>> GetHardwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null);
    }
}
