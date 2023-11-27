using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkShiftService
    {
        Task<WorkShift> Get(Guid id);

        Task<IEnumerable<WorkShift>> GetAll();

        Task Insert(WorkShift workShift);
        Task Update(WorkShift workShift);
        Task Delete(WorkShift workShift);

        Task<DataTablesStructs.ReturnedData<object>> GetAllWorkShiftsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
