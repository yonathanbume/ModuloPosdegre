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
    public class WorkerSecondSpecialtyService : IWorkerSecondSpecialtyService
    {
        private readonly IWorkerSecondSpecialtyRepository _workerSecondSpecialtyRepository;

        public WorkerSecondSpecialtyService(IWorkerSecondSpecialtyRepository workerSecondSpecialtyRepository)
        {
            _workerSecondSpecialtyRepository = workerSecondSpecialtyRepository;
        }

        public async Task<WorkerSecondSpecialty> Get(Guid workerSecondSpecialtyId)
        {
            return await _workerSecondSpecialtyRepository.Get(workerSecondSpecialtyId);
        }

        public async Task<IEnumerable<WorkerSecondSpecialty>> GetAll()
        {
            return await _workerSecondSpecialtyRepository.GetAll();
        }

        public async Task<List<WorkerSecondSpecialty>> GetAllByUserId(string userId)
        {
            return await _workerSecondSpecialtyRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerSecondSpecialty workerSecondSpecialty)
        {
            await _workerSecondSpecialtyRepository.Insert(workerSecondSpecialty);
        }

        public async Task Update(WorkerSecondSpecialty workerSecondSpecialty)
        {
            await _workerSecondSpecialtyRepository.Update(workerSecondSpecialty);
        }

        public async Task Delete(WorkerSecondSpecialty workerSecondSpecialty)
        {
            await _workerSecondSpecialtyRepository.Delete(workerSecondSpecialty);
        }

        public async Task<int> GetWorkerSecondSpecialtiesQuantity(string userId)
        {
            return await _workerSecondSpecialtyRepository.GetWorkerSecondSpecialtiesQuantity(userId);
        }

        public async Task<List<SecondSpecialtiesTemplate>> GetWorkerSecondSpecialtiesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerSecondSpecialtyRepository.GetWorkerSecondSpecialtiesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerSecondSpecialty> GetWithIncludes(Guid id)
            => await _workerSecondSpecialtyRepository.GetWithIncludes(id);
    }
}
