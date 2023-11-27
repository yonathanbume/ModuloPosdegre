using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IComputerStateService
    {
        //Generals
        Task<ComputerState> Get(Guid id);
        Task<IEnumerable<ComputerState>> GetAll();
        Task Insert(ComputerState computerState);
        Task Delete(ComputerState computerState);
        Task Update(ComputerState computerState);

        Task<DataTablesStructs.ReturnedData<object>> GetComputerStateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
