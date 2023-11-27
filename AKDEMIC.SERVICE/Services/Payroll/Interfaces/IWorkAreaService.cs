using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkAreaService
    {
        Task<IEnumerable<WorkArea>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<WorkArea> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task Insert(WorkArea workArea);
        Task Update(WorkArea workArea);
        Task DeleteById(Guid id);
    }
}
