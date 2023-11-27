using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWageItemService
    {
        
        Task<WageItem> Get(Guid id);
        Task<IEnumerable<WageItem>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(WageItem wageItem);
        Task Update(WageItem wageItem);
        Task DeleteById(Guid id);
    }
}
