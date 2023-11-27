using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerPersonalDocumentService : IWorkerPersonalDocumentService
    {
        private readonly IWorkerPersonalDocumentRepository _workerPersonalDocumentRepository;

        public WorkerPersonalDocumentService(IWorkerPersonalDocumentRepository workerPersonalDocumentRepository)
        {
            _workerPersonalDocumentRepository = workerPersonalDocumentRepository;
        }

        public async Task<WorkerPersonalDocument> Get(Guid id)
            => await _workerPersonalDocumentRepository.Get(id);

        public async Task<WorkerPersonalDocument> GetByUserId(string userId)
            => await _workerPersonalDocumentRepository.GetByUserId(userId);

        public async Task Insert(WorkerPersonalDocument entity)
            => await _workerPersonalDocumentRepository.Insert(entity);

        public async Task Update(WorkerPersonalDocument entity)
            => await _workerPersonalDocumentRepository.Update(entity);
    }
}
