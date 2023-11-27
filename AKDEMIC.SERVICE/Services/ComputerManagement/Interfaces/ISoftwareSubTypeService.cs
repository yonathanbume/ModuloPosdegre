using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface ISoftwareSubTypeService
    {
        //Generales
        Task Insert(SoftwareSubType softwareSubType);
        Task Update(SoftwareSubType softwareSubType);
        Task Delete(SoftwareSubType softwareSubType);
        Task DeleteById(Guid id);
        Task<SoftwareSubType> Get(Guid id);
        Task<IEnumerable<SoftwareSubType>> GetAll();

        Task<DataTablesStructs.ReturnedData<object>> GetSoftwareSubTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid TypeId, string searchValue = null);
    }
}
