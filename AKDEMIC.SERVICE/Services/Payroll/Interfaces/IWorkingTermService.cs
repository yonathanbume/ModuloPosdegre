using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkingTerm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkingTermService
    {
        Task<(IEnumerable<WorkingTerm> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);
        Task<IEnumerable<WorkingTerm>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<WorkingTerm> GetActive();
        Task<WorkingTerm> Get(Guid id);
        Task<int> MaxNumberByYearMonth(int year, int month);
        Task Insert(WorkingTerm workingTerm);
        Task Update(WorkingTerm workingTerm);
        Task DeleteById(Guid id);

        Task<bool> AnyActive();

        Task<WorkingTermTemplate> GetLastActive();
    }
}
