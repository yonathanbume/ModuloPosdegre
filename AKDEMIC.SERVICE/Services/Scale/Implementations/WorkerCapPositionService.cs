using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public  class WorkerCapPositionService : IWorkerCapPositionService
    {
        private readonly IWorkerCapPositionRepository _workerCapPositionRepository;

        public WorkerCapPositionService(IWorkerCapPositionRepository workerCapPositionRepository)
        {
            _workerCapPositionRepository = workerCapPositionRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _workerCapPositionRepository.AnyByCode(code, ignoredId);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _workerCapPositionRepository.AnyByName(name, ignoredId);

        public async Task Delete(WorkerCapPosition entity)
            => await _workerCapPositionRepository.Delete(entity);

        public async Task<WorkerCapPosition> Get(Guid id)
            => await _workerCapPositionRepository.Get(id);

        public async Task<IEnumerable<WorkerCapPosition>> GetAll(string search = null, bool? onlyActive = false)
            => await _workerCapPositionRepository.GetAll(search, onlyActive);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerCapPositionDataTable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue)
            => await _workerCapPositionRepository.GetWorkerCapPositionDataTable(sentParameters, status, searchValue);

        public async Task Insert(WorkerCapPosition entity)
            => await _workerCapPositionRepository.Insert(entity);

        public async Task Update(WorkerCapPosition entity)
            => await _workerCapPositionRepository.Update(entity);
    }
}
