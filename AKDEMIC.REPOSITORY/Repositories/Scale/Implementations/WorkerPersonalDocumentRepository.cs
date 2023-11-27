using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerPersonalDocumentRepository : Repository<WorkerPersonalDocument>, IWorkerPersonalDocumentRepository
    {
        public WorkerPersonalDocumentRepository(AkdemicContext context) : base(context) { }

        public async Task<WorkerPersonalDocument> GetByUserId(string userId)
            => await _context.WorkerPersonalDocuments.Where(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}
