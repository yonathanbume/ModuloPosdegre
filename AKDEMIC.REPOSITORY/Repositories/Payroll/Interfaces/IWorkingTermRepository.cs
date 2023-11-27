using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkingTerm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkingTermRepository : IRepository<WorkingTerm>
    {
        Task<(IEnumerable<WorkingTerm> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<WorkingTerm> GetActive();

        Task<int> MaxNumberByYearMonth(int year, int month);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<bool> AnyActive();

        Task<WorkingTermTemplate> GetLastActive();
    }
}
