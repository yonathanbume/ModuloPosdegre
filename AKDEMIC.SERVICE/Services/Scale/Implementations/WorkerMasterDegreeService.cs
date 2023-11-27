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
    public class WorkerMasterDegreeService : IWorkerMasterDegreeService
    {
        private readonly IWorkerMasterDegreeRepository _workerMasterDegreeRepository;

        public WorkerMasterDegreeService(IWorkerMasterDegreeRepository workerMasterDegreeRepository)
        {
            _workerMasterDegreeRepository = workerMasterDegreeRepository;
        }

        public async Task<WorkerMasterDegree> Get(Guid workerMasterDegreeId)
        {
            return await _workerMasterDegreeRepository.Get(workerMasterDegreeId);
        }

        public async Task<IEnumerable<WorkerMasterDegree>> GetAll()
        {
            return await _workerMasterDegreeRepository.GetAll();
        }

        public async Task<List<WorkerMasterDegree>> GetAllByUserId(string userId)
        {
            return await _workerMasterDegreeRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerMasterDegree workerMasterDegree)
        {
            await _workerMasterDegreeRepository.Insert(workerMasterDegree);
        }

        public async Task Update(WorkerMasterDegree workerMasterDegree)
        {
            await _workerMasterDegreeRepository.Update(workerMasterDegree);
        }

        public async Task Delete(WorkerMasterDegree workerMasterDegree)
        {
            await _workerMasterDegreeRepository.Delete(workerMasterDegree);
        }

        public Task<int> GetWorkerDoctoralDegreesQuantity(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetWorkerMasterDegreesQuantity(string userId)
        {
            return await _workerMasterDegreeRepository.GetWorkerMasterDegreesQuantity(userId);
        }

        public async Task<List<MasterDegreesTemplate>> GetWorkerMasterDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerMasterDegreeRepository.GetWorkerMasterDegreesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerMasterDegree> GetWithIncludes(Guid id)
            => await _workerMasterDegreeRepository.GetWithIncludes(id);
    }
}
