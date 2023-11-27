using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerFamilyInformationService : IWorkerFamilyInformationService
    {
        private readonly IWorkerFamilyInformationRepository _workerFamilyInformationRepository;

        public WorkerFamilyInformationService(IWorkerFamilyInformationRepository workerFamilyInformationRepository)
        {
            _workerFamilyInformationRepository = workerFamilyInformationRepository;
        }
        public Task AddRange(IEnumerable<WorkerFamilyInformation> workerFamilyInformations)
            => _workerFamilyInformationRepository.AddRange(workerFamilyInformations);

        public Task<WorkerFamilyInformation> Get(Guid id)
            => _workerFamilyInformationRepository.Get(id);

        public Task<IEnumerable<WorkerFamilyInformation>> GetAll()
            => _workerFamilyInformationRepository.GetAll();

        public Task<List<WorkerFamilyInformation>> GetAllByUserId(string userId)
            => _workerFamilyInformationRepository.GetAllByUserId(userId);
        public Task<object> GetFamilyMembers(Guid workerLaborInformationId)
            => _workerFamilyInformationRepository.GetFamilyMembers(workerLaborInformationId);

        public Task Insert(WorkerFamilyInformation workerFamilyInformation)
            => _workerFamilyInformationRepository.Insert(workerFamilyInformation);

        public Task InsertRange(IEnumerable<WorkerFamilyInformation> workerFamilyInformations)
            => _workerFamilyInformationRepository.InsertRange(workerFamilyInformations);

        public Task RemoveRangeByUserId(string userId)
            => _workerFamilyInformationRepository.RemoveRangeByUserId(userId);

        public Task Update(WorkerFamilyInformation workerFamilyInformation)
            => _workerFamilyInformationRepository.Update(workerFamilyInformation);

    }
}
