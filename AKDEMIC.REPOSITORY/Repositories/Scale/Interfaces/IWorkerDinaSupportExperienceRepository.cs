using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerDinaSupportExperienceRepository : IRepository<WorkerDinaSupportExperience>
    {
        Task UpdateExperience(List<byte> types, Guid workerDinaId);
    }
}
