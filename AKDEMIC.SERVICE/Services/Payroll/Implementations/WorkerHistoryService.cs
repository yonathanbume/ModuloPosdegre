using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkerHistoryService : IWorkerHistoryService
    {
        private readonly IWorkerHistoryRepository _workerHistoryRepository;

        public WorkerHistoryService(IWorkerHistoryRepository workerHistoryRepository)
        {
            _workerHistoryRepository = workerHistoryRepository;
        }

        public async Task DeleteById(Guid id)
            => await _workerHistoryRepository.DeleteById(id);

        public async Task FireWorkerById(Guid workerId)
            => await _workerHistoryRepository.FireWorkerById(workerId);

        public async Task<WorkerHistory> Get(Guid id)
            => await _workerHistoryRepository.Get(id);

        public async Task<IEnumerable<WorkerHistory>> GetAll()
            => await _workerHistoryRepository.GetAll();

        public async Task<(IEnumerable<WorkerHistory> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter, Guid workerId)
            => await _workerHistoryRepository.GetAllByPaginationParameter(paginationParameter, workerId);

        public async Task Insert(WorkerHistory workerWorkTerm)
            => await _workerHistoryRepository.Insert(workerWorkTerm);

        public async Task RestoreWorkerById(Guid workerId)
            => await _workerHistoryRepository.RestoreWorkerById(workerId);

        public async Task Update(WorkerHistory workerWorkTerm)
            => await _workerHistoryRepository.Update(workerWorkTerm);
    }
}
