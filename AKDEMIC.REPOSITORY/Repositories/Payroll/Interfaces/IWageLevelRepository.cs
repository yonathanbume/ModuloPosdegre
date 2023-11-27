using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWageLevelRepository:IRepository<WageLevel>
    {
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllWageLevelsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
