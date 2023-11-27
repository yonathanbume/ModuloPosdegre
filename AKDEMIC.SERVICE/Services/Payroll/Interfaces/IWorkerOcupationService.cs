using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkerOcupationService
    {
        Task<(IEnumerable<WorkerOcupation> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);
        Task<IEnumerable<WorkerOcupation>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<WorkerOcupation> Get(Guid id);
        Task Insert(WorkerOcupation workArea);
        Task Update(WorkerOcupation workArea);
        Task DeleteById(Guid id);
    }
}
