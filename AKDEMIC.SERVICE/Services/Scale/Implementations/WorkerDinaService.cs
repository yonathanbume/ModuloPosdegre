using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerDinaService : IWorkerDinaService
    {
        private readonly IWorkerDinaRepository _workerDinaRepository;

        public WorkerDinaService(IWorkerDinaRepository workerDinaRepository)
        {
            _workerDinaRepository = workerDinaRepository;
        }
        public async Task<WorkerDina> GetByUserId(string userId)
            => await _workerDinaRepository.GetByUserId(userId);

        public Task<int> GetTeacherInDinaCount()
            => _workerDinaRepository.GetTeacherInDinaCount();

        public async Task Insert(WorkerDina entity)
            => await _workerDinaRepository.Insert(entity);

        public async Task Update(WorkerDina entity)
            => await _workerDinaRepository.Update(entity);
    }
}
