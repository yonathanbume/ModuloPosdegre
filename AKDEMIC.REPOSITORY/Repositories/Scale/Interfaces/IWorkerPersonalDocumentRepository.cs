using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerPersonalDocumentRepository : IRepository<WorkerPersonalDocument>
    {
        Task<WorkerPersonalDocument> GetByUserId(string userId);
    }
}
