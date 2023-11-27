using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerLaborTermInformationService
    {
        Task<WorkerLaborTermInformation> GetByUserIdAndActiveTerm(string userId);
        Task Insert(WorkerLaborTermInformation newWorkerLaborTermInformation);
        Task<WorkerLaborTermInformation> Add(WorkerLaborTermInformation newWorkerLaborTermInformation);
        Task Update(WorkerLaborTermInformation workerLaborTermInformation);
        Task<List<WorkerLaborTermInformation>> GetAllWithManagementPosition();
    }
}
