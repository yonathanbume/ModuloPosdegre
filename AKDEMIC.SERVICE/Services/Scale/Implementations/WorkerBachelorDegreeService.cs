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
    public class WorkerBachelorDegreeService : IWorkerBachelorDegreeService
    {
        private readonly IWorkerBachelorDegreeRepository _workerBachelorDegreeRepository;

        public WorkerBachelorDegreeService(IWorkerBachelorDegreeRepository workerBachelorDegreeRepository)
        {
            _workerBachelorDegreeRepository = workerBachelorDegreeRepository;
        }

        public async Task<WorkerBachelorDegree> Get(Guid workerBachelorDegreeId)
        {
            return await _workerBachelorDegreeRepository.Get(workerBachelorDegreeId);
        }

        public async Task<IEnumerable<WorkerBachelorDegree>> GetAll()
        {
            return await _workerBachelorDegreeRepository.GetAll();
        }

        public async Task<List<WorkerBachelorDegree>> GetAllByUserId(string userId)
        {
            return await _workerBachelorDegreeRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerBachelorDegree workerBachelorDegree)
        {
            await _workerBachelorDegreeRepository.Insert(workerBachelorDegree);
        }

        public async Task Update(WorkerBachelorDegree workerBachelorDegree)
        {
            await _workerBachelorDegreeRepository.Update(workerBachelorDegree);
        }

        public async Task Delete(WorkerBachelorDegree workerBachelorDegree)
        {
            await _workerBachelorDegreeRepository.Delete(workerBachelorDegree);
        }

        public async Task<int> GetWorkerBachelorDegreesQuantity(string userId)
        {
            return await _workerBachelorDegreeRepository.GetWorkerBachelorDegreesQuantity(userId);
        }

        public async Task<List<BachelorDegreesTemplate>> GetWorkerBachelorDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerBachelorDegreeRepository.GetWorkerBachelorDegreesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerBachelorDegree> GetWithIncludes(Guid id)
            => await _workerBachelorDegreeRepository.GetWithIncludes(id);
    }
}
