using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class WorkerDinaSupportExperienceService : IWorkerDinaSupportExperienceService
    {
        private readonly IWorkerDinaSupportExperienceRepository _workerDinaSupportExperienceRepository;

        public WorkerDinaSupportExperienceService(IWorkerDinaSupportExperienceRepository workerDinaSupportExperienceRepository)
        {
            _workerDinaSupportExperienceRepository = workerDinaSupportExperienceRepository;
        }

        public async Task UpdateExperience(List<byte> types, Guid workerDinaId)
            => await _workerDinaSupportExperienceRepository.UpdateExperience(types, workerDinaId);
    }
}
