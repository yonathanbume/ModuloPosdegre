using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.CORE.Structs;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollRepository : IRepository<ENTITIES.Models.Payroll.Payroll>
    {
        Task<(IEnumerable<ENTITIES.Models.Payroll.Payroll> pagedList, int count)> 
            GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<ENTITIES.Models.Payroll.Payroll> FindByCode(string code);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
