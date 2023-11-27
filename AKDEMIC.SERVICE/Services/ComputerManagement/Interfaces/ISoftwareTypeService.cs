using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface ISoftwareTypeService
    {
        //Generales
        Task Insert(SoftwareType hardwareType);
        Task Update(SoftwareType hardwareType);
        Task Delete(SoftwareType hardwareType);
        Task DeleteById(Guid id);
        Task<SoftwareType> Get(Guid id);
        Task<IEnumerable<SoftwareType>> GetAll();

        Task<DataTablesStructs.ReturnedData<object>> GetSoftwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);


    }
}
