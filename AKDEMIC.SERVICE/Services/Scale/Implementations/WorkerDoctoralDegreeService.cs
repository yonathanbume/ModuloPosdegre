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
    public class WorkerDoctoralDegreeService : IWorkerDoctoralDegreeService
    {
        private readonly IWorkerDoctoralDegreeRepository _workerDoctoralDegreeRepository;

        public WorkerDoctoralDegreeService(IWorkerDoctoralDegreeRepository workerDoctoralDegreeRepository)
        {
            _workerDoctoralDegreeRepository = workerDoctoralDegreeRepository;
        }

        public async Task<WorkerDoctoralDegree> Get(Guid workerDoctoralDegreeId)
        {
            return await _workerDoctoralDegreeRepository.Get(workerDoctoralDegreeId);
        }

        public async Task<IEnumerable<WorkerDoctoralDegree>> GetAll()
        {
            return await _workerDoctoralDegreeRepository.GetAll();
        }

        public async Task<List<WorkerDoctoralDegree>> GetAllByUserId(string userId)
        {
            return await _workerDoctoralDegreeRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerDoctoralDegree workerDoctoralDegree)
        {
            await _workerDoctoralDegreeRepository.Insert(workerDoctoralDegree);
        }

        public async Task Update(WorkerDoctoralDegree workerDoctoralDegree)
        {
            await _workerDoctoralDegreeRepository.Update(workerDoctoralDegree);
        }

        public async Task Delete(WorkerDoctoralDegree workerDoctoralDegree)
        {
            await _workerDoctoralDegreeRepository.Delete(workerDoctoralDegree);
        }

        public async Task<int> GetWorkerDoctoralDegreesQuantity(string userId)
        {
            return await _workerDoctoralDegreeRepository.GetWorkerDoctoralDegreesQuantity(userId);
        }

        public async Task<List<DoctoralDegreesTemplate>> GetWorkerDoctoralDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerDoctoralDegreeRepository.GetWorkerDoctoralDegreesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerDoctoralDegree> GetWithIncludes(Guid id)
            => await _workerDoctoralDegreeRepository.GetWithIncludes(id);
    }
}
