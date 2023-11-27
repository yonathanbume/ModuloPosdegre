using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerDinaSupportExperienceService
    {
        Task UpdateExperience(List<byte> types, Guid workerDinaId);
    }
}
