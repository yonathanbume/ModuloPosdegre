using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkerOcupationRepository : IRepository<WorkerOcupation>
    {
        Task<(IEnumerable<WorkerOcupation> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter);

        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
