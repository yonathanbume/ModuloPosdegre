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
    public class WorkerTrainingService : IWorkerTrainingService
    {
        private readonly IWorkerTrainingRepository _workerTrainingRepository;

        public WorkerTrainingService(IWorkerTrainingRepository workerTrainingRepository)
        {
            _workerTrainingRepository = workerTrainingRepository;
        }

        public async Task Delete(WorkerTraining entity)
            => await _workerTrainingRepository.Delete(entity);

        public async Task<WorkerTraining> Get(Guid id)
            => await _workerTrainingRepository.Get(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetUsersWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, int? userType = null, string searchValue = null)
            => _workerTrainingRepository.GetUsersWorkerTrainingDatatble(parameters, userType, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, string userId, string searchValue)
            => await _workerTrainingRepository.GetWorkerTrainingDatatble(parameters, userId, searchValue);

        public async Task Insert(WorkerTraining entity)
            => await _workerTrainingRepository.Insert(entity);

        public async Task Update(WorkerTraining entity)
            => await _workerTrainingRepository.Update(entity);
    }
}
