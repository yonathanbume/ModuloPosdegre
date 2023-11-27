using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerFamilyInformationService
    {
        Task<WorkerFamilyInformation> Get(Guid id);
        Task<object> GetFamilyMembers(Guid workerLaborInformationId);
        Task RemoveRangeByUserId(string userId);
        Task Insert(WorkerFamilyInformation workerFamilyInformation);
        Task Update(WorkerFamilyInformation workerFamilyInformation);
        Task InsertRange(IEnumerable<WorkerFamilyInformation> workerFamilyInformations);
        Task AddRange(IEnumerable<WorkerFamilyInformation> workerFamilyInformations);
        Task<IEnumerable<WorkerFamilyInformation>> GetAll();
        Task<List<WorkerFamilyInformation>> GetAllByUserId(string userId);
    }
}
