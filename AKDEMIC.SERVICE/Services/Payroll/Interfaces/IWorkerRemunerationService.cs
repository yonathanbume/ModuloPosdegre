using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkerRemunerationService
    {
        Task<WorkerRemuneration> Get(Guid id);
        Task<IEnumerable<WorkerRemuneration>> GetAll();
        Task Insert(WorkerRemuneration workerRemuneration);
        Task Update(WorkerRemuneration workerRemuneration);
        Task Delete(WorkerRemuneration workerRemuneration);
        Task<List<WorkerRemunerationTemplate>> GetAllToCloseWorkingTerm(Guid workingTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllWorkerRemunerationsDatatable(DataTablesStructs.SentParameters sentParameters,Guid workerId ,Guid? administrativeTableId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetFilteredWorkerRemunerationsDatatable(Guid workerId, Guid startWorkingTermId, Guid endWorkingTermId);
    }
}
