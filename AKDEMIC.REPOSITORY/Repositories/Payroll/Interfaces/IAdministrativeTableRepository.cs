using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IAdministrativeTableRepository: IRepository<AdministrativeTable> 
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllAdministrativeTablesDatatable(DataTablesStructs.SentParameters sentParameters, int type ,string searchValue = null);
        Task<object> GetSelect2(int? type = null);
        Task<bool> AnyByCode(string name, int type,Guid? id = null);
    }
}
