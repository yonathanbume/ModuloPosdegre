using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerBankAccountInformationService : IWorkerBankAccountInformationService
    {
        private readonly IWorkerBankAccountInformationRepository _workerBankAccountInformationRepository;

        public WorkerBankAccountInformationService(IWorkerBankAccountInformationRepository workerBankAccountInformationRepository)
        {
            _workerBankAccountInformationRepository = workerBankAccountInformationRepository;
        }

        public async Task AddRange(List<WorkerBankAccountInformation> workerBankAccountInformations)
            => await _workerBankAccountInformationRepository.AddRange(workerBankAccountInformations);

        public async Task Delete(WorkerBankAccountInformation workerBankAccountInformation)
            => await _workerBankAccountInformationRepository.Delete(workerBankAccountInformation);

        public async Task<WorkerBankAccountInformation> Get(Guid id)
            => await _workerBankAccountInformationRepository.Get(id);

        public async Task<IEnumerable<WorkerBankAccountInformation>> GetAll()
            => await _workerBankAccountInformationRepository.GetAll();

        public Task<List<WorkerBankAccountInformation>> GetAllByWorker(Guid workerLaborInformationId)
            => _workerBankAccountInformationRepository.GetAllByWorker(workerLaborInformationId);

        public async Task Insert(WorkerBankAccountInformation workerBankAccountInformation)
            => await _workerBankAccountInformationRepository.Insert(workerBankAccountInformation);

        public void RemoveRange(List<WorkerBankAccountInformation> workerBankAccountInformations)
            => _workerBankAccountInformationRepository.RemoveRange(workerBankAccountInformations);

        public async Task Update(WorkerBankAccountInformation workerBankAccountInformation)
            => await _workerBankAccountInformationRepository.Update(workerBankAccountInformation);
    }
}
