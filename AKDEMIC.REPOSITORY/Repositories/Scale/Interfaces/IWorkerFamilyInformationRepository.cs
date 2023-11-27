using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerFamilyInformationRepository:IRepository<WorkerFamilyInformation>
    {
        Task RemoveRangeByUserId(string userId);
        Task<List<WorkerFamilyInformation>> GetAllByUserId(string userId);
        Task<object> GetFamilyMembers(Guid workerLaborInformationId);
    }
}
