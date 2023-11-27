using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerLaborTermInformationService: IWorkerLaborTermInformationService
    {
        private readonly IWorkerLaborTermInformationRepository _workerLaborTermInformationRepository;
        public WorkerLaborTermInformationService(IWorkerLaborTermInformationRepository workerLaborTermInformationRepository)
        {
            _workerLaborTermInformationRepository = workerLaborTermInformationRepository;
        }

        public async Task<WorkerLaborTermInformation> Add(WorkerLaborTermInformation newWorkerLaborTermInformation)
            => await _workerLaborTermInformationRepository.Add(newWorkerLaborTermInformation);

        public async Task<List<WorkerLaborTermInformation>> GetAllWithManagementPosition()
        {
            return await _workerLaborTermInformationRepository.GetAllWithManagementPosition();
        }

        public async Task<WorkerLaborTermInformation> GetByUserIdAndActiveTerm(string userId)
        {
            return await _workerLaborTermInformationRepository.GetByUserIdAndActiveTerm(userId);
        }

        public async Task Insert(WorkerLaborTermInformation newWorkerLaborTermInformation)
        {
            await _workerLaborTermInformationRepository.Insert(newWorkerLaborTermInformation);
        }

        public async Task Update(WorkerLaborTermInformation workerLaborTermInformation)
        {
            await _workerLaborTermInformationRepository.Update(workerLaborTermInformation);
        }
    }
}
