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
    public class WorkerTechnicalStudyService : IWorkerTechnicalStudyService
    {
        private readonly IWorkerTechnicalStudyRepository _workerTechnicalStudyRepository;

        public WorkerTechnicalStudyService(IWorkerTechnicalStudyRepository workerTechnicalStudyRepository)
        {
            _workerTechnicalStudyRepository = workerTechnicalStudyRepository;
        }

        public async Task<WorkerTechnicalStudy> Get(Guid workerTechnicalStudyId)
        {
            return await _workerTechnicalStudyRepository.Get(workerTechnicalStudyId);
        }

        public async Task<IEnumerable<WorkerTechnicalStudy>> GetAll()
        {
            return await _workerTechnicalStudyRepository.GetAll();
        }

        public async Task<List<WorkerTechnicalStudy>> GetAllByUserId(string userId)
        {
            return await _workerTechnicalStudyRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerTechnicalStudy workerTechnicalStudy)
        {
            await _workerTechnicalStudyRepository.Insert(workerTechnicalStudy);
        }

        public async Task Update(WorkerTechnicalStudy workerTechnicalStudy)
        {
            await _workerTechnicalStudyRepository.Update(workerTechnicalStudy);
        }

        public async Task Delete(WorkerTechnicalStudy workerTechnicalStudy)
        {
            await _workerTechnicalStudyRepository.Delete(workerTechnicalStudy);
        }

        public async Task<int> GetWorkerTechnicalStudiesQuantity(string userId)
        {
            return await _workerTechnicalStudyRepository.GetWorkerTechnicalStudiesQuantity(userId);
        }

        public async Task<List<TechnicalStudiesTemplate>> GetWorkerTechnicalStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerTechnicalStudyRepository.GetWorkerTechnicalStudiesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerTechnicalStudy> GetWithIncludes(Guid id)
            => await _workerTechnicalStudyRepository.GetWithIncludes(id);
    }
}
