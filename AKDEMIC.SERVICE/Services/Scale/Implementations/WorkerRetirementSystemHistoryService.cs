using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerRetirementSystemHistoryService : IWorkerRetirementSystemHistoryService
    {
        private readonly IWorkerRetirementSystemHistoryRepository _workerRetirementSystemHistoryRepository;

        public WorkerRetirementSystemHistoryService(IWorkerRetirementSystemHistoryRepository workerRetirementSystemHistoryRepository)
        {
            _workerRetirementSystemHistoryRepository = workerRetirementSystemHistoryRepository;
        }

        public async Task<bool> AnyActiveByUser(string userId, Guid? ignoredId = null)
            => await _workerRetirementSystemHistoryRepository.AnyActiveByUser(userId, ignoredId);

        public async Task<WorkerRetirementSystemHistory> Get(Guid id)
            => await _workerRetirementSystemHistoryRepository.Get(id);

        public async Task<WorkerRetirementSystemHistory> GetActiveRetirementSystem(string userId)
            => await _workerRetirementSystemHistoryRepository.GetActiveRetirementSystem(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerRetirementSystemHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _workerRetirementSystemHistoryRepository.GetWorkerRetirementSystemHistoryDatatable(sentParameters, userId);

        public async Task InsertWorkerRetirementSystem(WorkerRetirementSystemHistory entity)
            => await _workerRetirementSystemHistoryRepository.InsertWorkerRetirementSystem(entity);

        public async Task Update(WorkerRetirementSystemHistory entity)
            => await _workerRetirementSystemHistoryRepository.Update(entity);
    }
}
