using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface ISoftwareService
    {
        //Generales
        Task Insert(Software software);
        Task Update(Software software);
        Task Delete(Software software);
        Task DeleteById(Guid id);
        Task<Software> Get(Guid id);
        Task<IEnumerable<Software>> GetAll();

        Task<IEnumerable<Software>> GetSoftwaresByComputer(Guid computerId);
        Task<DataTablesStructs.ReturnedData<object>> GetSoftwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null);
    }
}
