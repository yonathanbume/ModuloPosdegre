using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkerRemunerationService : IWorkerRemunerationService
    {
        private readonly IWorkerRemunerationRepository _workerRemunerationRepository;

        public WorkerRemunerationService(IWorkerRemunerationRepository workerRemunerationRepository)
        {
            _workerRemunerationRepository = workerRemunerationRepository;
        }

        public async Task Delete(WorkerRemuneration workerRemuneration)
            => await _workerRemunerationRepository.Delete(workerRemuneration);

        public async Task<WorkerRemuneration> Get(Guid id)
            => await _workerRemunerationRepository.Get(id);

        public async Task<IEnumerable<WorkerRemuneration>> GetAll()
            => await _workerRemunerationRepository.GetAll();

        public Task<List<WorkerRemunerationTemplate>> GetAllToCloseWorkingTerm(Guid workingTermId)
            => _workerRemunerationRepository.GetAllToCloseWorkingTerm(workingTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllWorkerRemunerationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid workerId,Guid? conceptTypeId = null, string searchValue = null)
            => await _workerRemunerationRepository.GetAllWorkerRemunerationsDatatable(sentParameters, workerId , conceptTypeId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetFilteredWorkerRemunerationsDatatable(Guid workerId, Guid startWorkingTermId, Guid endWorkingTermId)
            => await _workerRemunerationRepository.GetFilteredWorkerRemunerationsDatatable(workerId, startWorkingTermId, endWorkingTermId);

        public async Task Insert(WorkerRemuneration workerRemuneration)
            => await _workerRemunerationRepository.Insert(workerRemuneration);

        public async Task Update(WorkerRemuneration workerRemuneration)
            => await _workerRemunerationRepository.Update(workerRemuneration);
    }
}
