using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerPersonalDocumentService
    {
        Task<WorkerPersonalDocument> Get(Guid id);
        Task<WorkerPersonalDocument> GetByUserId(string userId);
        Task Insert(WorkerPersonalDocument entity);
        Task Update(WorkerPersonalDocument entity);
    }
}
