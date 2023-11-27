using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IPayrollClassRepository : IRepository<PayrollClass>
    {
        Task<(IEnumerable<PayrollClass> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<PayrollClass> FindByCode(string code);
    }
}
