using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IPayrollService
    {
        Task<IEnumerable<ENTITIES.Models.Payroll.Payroll>> GetAll();

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<(IEnumerable<ENTITIES.Models.Payroll.Payroll> pagedList, int count)> 
            GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<ENTITIES.Models.Payroll.Payroll> Get(Guid id);

        Task<ENTITIES.Models.Payroll.Payroll> FindByCode(string code);

        Task Insert(ENTITIES.Models.Payroll.Payroll payroll);

        Task Update(ENTITIES.Models.Payroll.Payroll payroll);

        Task DeleteById(Guid id);

        Task Delete(ENTITIES.Models.Payroll.Payroll payroll);
    }
}
