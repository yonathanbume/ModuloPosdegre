using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkerOcupationService : IWorkerOcupationService
    {
        private readonly IWorkerOcupationRepository _workerOcupationRepository;

        public WorkerOcupationService(IWorkerOcupationRepository workerOcupationRepository)
        {
            _workerOcupationRepository = workerOcupationRepository;
        }

        public async Task DeleteById(Guid id)
            => await _workerOcupationRepository.DeleteById(id);

        public async Task<WorkerOcupation> Get(Guid id)
           => await _workerOcupationRepository.Get(id);

        public async Task<IEnumerable<WorkerOcupation>> GetAll()
            => await _workerOcupationRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                    => _workerOcupationRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task<(IEnumerable<WorkerOcupation> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
            => await _workerOcupationRepository.GetAllByPaginationParameter(paginationParameter);

        public async Task Insert(WorkerOcupation workArea)
            => await _workerOcupationRepository.Insert(workArea);

        public async Task Update(WorkerOcupation workArea)
            => await _workerOcupationRepository.Update(workArea);
    }
}
