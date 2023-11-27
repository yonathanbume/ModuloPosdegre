using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerManagementPositionService : IWorkerManagementPositionService
    {
        private readonly IWorkerManagementPositionRepository _workerManagementPositionRepository;

        public WorkerManagementPositionService(IWorkerManagementPositionRepository workerManagementPositionRepository)
        {
            _workerManagementPositionRepository = workerManagementPositionRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _workerManagementPositionRepository.AnyByCode(code, ignoredId);

        public async Task<bool> AnyByName(string name, Guid dependencyId ,Guid? ignoredId = null)
            => await _workerManagementPositionRepository.AnyByName(name,dependencyId ,ignoredId);

        public async Task Delete(WorkerManagementPosition entity)
            => await _workerManagementPositionRepository.Delete(entity);

        public async Task<WorkerManagementPosition> Get(Guid id)
            => await _workerManagementPositionRepository.Get(id);

        public async Task<IEnumerable<WorkerManagementPosition>> GetAll(string search, bool? onlyActive)
            => await _workerManagementPositionRepository.GetAll(search, onlyActive);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerManagementPositionDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchValue = null)
            => await _workerManagementPositionRepository.GetWorkerManagementPositionDatatable(sentParameters, status, searchValue);

        public async Task Insert(WorkerManagementPosition entity)
            => await _workerManagementPositionRepository.Insert(entity);

        public async Task Update(WorkerManagementPosition entity)
            => await _workerManagementPositionRepository.Update(entity);
    }
}
