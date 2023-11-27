using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IComputerTypeService
    {
        //Generals
        Task<ComputerType> Get(Guid id);
        Task<IEnumerable<ComputerType>> GetAll();
        Task Insert(ComputerType computerType);
        Task Delete(ComputerType computerType);
        Task Update(ComputerType computerType);

        Task<DataTablesStructs.ReturnedData<object>> GetComputerTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
