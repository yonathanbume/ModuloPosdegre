using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWageLevelService
    {
        Task<WageLevel> Get(Guid id);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<IEnumerable<WageLevel>> GetAll();
        Task Insert(WageLevel wageLevel);
        Task Update(WageLevel wageLevel);
        Task Delete(WageLevel wageLevel);

        Task<DataTablesStructs.ReturnedData<object>> GetAllWageLevelsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
