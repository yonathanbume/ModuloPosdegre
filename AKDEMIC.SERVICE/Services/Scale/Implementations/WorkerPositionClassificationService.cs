using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerPositionClassificationService : IWorkerPositionClassificationService
    {
        private readonly IWorkerPositionClassificationRepository _workerPositionClassificationRepository;

        public WorkerPositionClassificationService(IWorkerPositionClassificationRepository workerPositionClassificationRepository)
        {
            _workerPositionClassificationRepository = workerPositionClassificationRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _workerPositionClassificationRepository.AnyByName(name, ignoredId);

        public async Task Delete(WorkerPositionClassification entity)
            => await _workerPositionClassificationRepository.Delete(entity);

        public async Task<WorkerPositionClassification> Get(Guid id)
            => await _workerPositionClassificationRepository.Get(id);

        public async Task<IEnumerable<WorkerPositionClassification>> GetAll(byte? status, string search = null)
            => await _workerPositionClassificationRepository.GetAll(status, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerPositionClassificationDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null)
            => await _workerPositionClassificationRepository.GetWorkerPositionClassificationDatatable(sentParameters, status, searchvalue);

        public async Task Insert(WorkerPositionClassification entity)
            => await _workerPositionClassificationRepository.Insert(entity);

        public async Task Update(WorkerPositionClassification entity)
            => await _workerPositionClassificationRepository.Update(entity);
    }
}
