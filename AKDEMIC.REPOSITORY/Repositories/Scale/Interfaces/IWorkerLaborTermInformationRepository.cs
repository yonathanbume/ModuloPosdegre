using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerLaborTermInformationRepository:IRepository<WorkerLaborTermInformation>
    {
        Task<WorkerLaborTermInformation> GetByUserIdAndActiveTerm(string userId);
        Task<List<WorkerLaborTermInformation>> GetAllWithManagementPosition();
    }
}
