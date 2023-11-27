using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerDiplomateService : IWorkerDiplomateService
    {
        private readonly IWorkerDiplomateRepository _workerDiplomateRepository;

        public WorkerDiplomateService(IWorkerDiplomateRepository workerDiplomateRepository)
        {
            _workerDiplomateRepository = workerDiplomateRepository;
        }

        public async Task<WorkerDiplomate> Get(Guid workerDiplomateId)
        {
            return await _workerDiplomateRepository.Get(workerDiplomateId);
        }

        public async Task<IEnumerable<WorkerDiplomate>> GetAll()
        {
            return await _workerDiplomateRepository.GetAll();
        }

        public async Task<List<WorkerDiplomate>> GetAllByUserId(string userId)
        {
            return await _workerDiplomateRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerDiplomate workerDiplomate)
        {
            await _workerDiplomateRepository.Insert(workerDiplomate);
        }

        public async Task Update(WorkerDiplomate workerDiplomate)
        {
            await _workerDiplomateRepository.Update(workerDiplomate);
        }

        public async Task Delete(WorkerDiplomate workerDiplomate)
        {
            await _workerDiplomateRepository.Delete(workerDiplomate);
        }

        public async Task<int> GetWorkerDiplomatesQuantity(string userId)
        {
            return await _workerDiplomateRepository.GetWorkerDiplomatesQuantity(userId);
        }

        public async Task<List<DiplomatesTemplate>> GetWorkerDiplomatesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerDiplomateRepository.GetWorkerDiplomatesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerDiplomate> GetWithIncludes(Guid id)
            => await _workerDiplomateRepository.GetWithIncludes(id);
    }
}
