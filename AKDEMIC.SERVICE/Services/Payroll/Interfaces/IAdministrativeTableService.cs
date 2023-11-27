using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IAdministrativeTableService
    {
        Task<AdministrativeTable> Get(Guid id);
        Task<bool> AnyByCode(string code, int type,Guid? id = null);
        Task<IEnumerable<AdministrativeTable>> GetAll();
        Task<object> GetSelect2(int? type = null);
        Task Insert(AdministrativeTable administrativeTable);
        Task Update(AdministrativeTable administrativeTable);
        Task Delete(AdministrativeTable administrativeTable);

        Task<DataTablesStructs.ReturnedData<object>> GetAllAdministrativeTablesDatatable(DataTablesStructs.SentParameters sentParameters, int type,string searchValue = null);
    }
}
