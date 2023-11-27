using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IHardwareTypeService
    {
        //Generales
        Task Insert(HardwareType hardwareType);
        Task Update(HardwareType hardwareType);
        Task Delete(HardwareType hardwareType);
        Task DeleteById(Guid id);
        Task<HardwareType> Get(Guid id);
        Task<IEnumerable<HardwareType>> GetAll();

        Task<DataTablesStructs.ReturnedData<object>> GetHardwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

    }
}
