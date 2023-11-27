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
    public class WorkerProfessionalTitleService : IWorkerProfessionalTitleService
    {
        private readonly IWorkerProfessionalTitleRepository _workerProfessionalTitleRepository;

        public WorkerProfessionalTitleService(IWorkerProfessionalTitleRepository workerProfessionalTitleRepository)
        {
            _workerProfessionalTitleRepository = workerProfessionalTitleRepository;
        }

        public async Task<WorkerProfessionalTitle> Get(Guid workerProfessionalTitleId)
        {
            return await _workerProfessionalTitleRepository.Get(workerProfessionalTitleId);
        }

        public async Task<IEnumerable<WorkerProfessionalTitle>> GetAll()
        {
            return await _workerProfessionalTitleRepository.GetAll();
        }

        public async Task<List<WorkerProfessionalTitle>> GetAllByUserId(string userId)
        {
            return await _workerProfessionalTitleRepository.GetAllByUserId(userId);
        }

        public async Task Insert(WorkerProfessionalTitle workerProfessionalTitle)
        {
            await _workerProfessionalTitleRepository.Insert(workerProfessionalTitle);
        }

        public async Task Update(WorkerProfessionalTitle workerProfessionalTitle)
        {
            await _workerProfessionalTitleRepository.Update(workerProfessionalTitle);
        }

        public async Task Delete(WorkerProfessionalTitle workerProfessionalTitle)
        {
            await _workerProfessionalTitleRepository.Delete(workerProfessionalTitle);
        }

        public async Task<int> GetWorkerProfessionalTitlesQuantity(string userId)
        {
            return await _workerProfessionalTitleRepository.GetWorkerProfessionalTitlesQuantity(userId);
        }

        public async Task<List<ProfessionalTitlesTemplate>> GetWorkerProfessionalTitlesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _workerProfessionalTitleRepository.GetWorkerProfessionalTitlesByPaginationParameters(userId, paginationParameter);
        }

        public async Task<WorkerProfessionalTitle> GetWithIncludes(Guid id)
            => await _workerProfessionalTitleRepository.GetWithIncludes(id);
    }
}
