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
    public class WorkerOtherStudyService : IWorkerOtherStudyService
    {
        private readonly IWorkerOtherStudyRepository _workerOtherStudyRepository;

        public WorkerOtherStudyService(IWorkerOtherStudyRepository workerOtherStudyRepository)
        {
            _workerOtherStudyRepository = workerOtherStudyRepository;
        }

        public async Task<WorkerOtherStudy> Get(Guid workerOtherStudyId)
        {
            return await _workerOtherStudyRepository.Get(workerOtherStudyId);
        }

        public async Task<IEnumerable<WorkerOtherStudy>> GetAll()
        {
            return await _workerOtherStudyRepository.GetAll();
        }

        public async Task<List<WorkerOtherStudy>> GetAllByUserId(string userId)
        {
            return await _workerOtherStudyRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerOtherStudy workerOtherStudy)
        {
            await _workerOtherStudyRepository.Insert(workerOtherStudy);
        }

        public async Task Update(WorkerOtherStudy workerOtherStudy)
        {
            await _workerOtherStudyRepository.Update(workerOtherStudy);
        }

        public async Task Delete(WorkerOtherStudy workerOtherStudy)
        {
            await _workerOtherStudyRepository.Delete(workerOtherStudy);
        }

        public async Task<int> GetWorkerOtherStudiesQuantity(string userId)
        {
            return await _workerOtherStudyRepository.GetWorkerOtherStudiesQuantity(userId);
        }

        public async Task<List<OtherStudiesTemplate>> GetWorkerOtherStudiesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerOtherStudyRepository.GetWorkerOtherStudiesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerOtherStudy> GetWithIncludes(Guid id)
            => await _workerOtherStudyRepository.GetWithIncludes(id);
    }
}
