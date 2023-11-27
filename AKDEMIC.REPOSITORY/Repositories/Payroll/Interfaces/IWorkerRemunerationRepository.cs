using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces
{
    public interface IWorkerRemunerationRepository : IRepository<WorkerRemuneration>
    {
        Task<List<WorkerRemunerationTemplate>> GetAllToCloseWorkingTerm(Guid workingTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllWorkerRemunerationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid workerId , Guid? administrativeTableId,string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetFilteredWorkerRemunerationsDatatable(Guid workerId, Guid startWorkingTermId, Guid endWorkingTermId);
    }
}
