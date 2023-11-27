using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerLaborTermInformationRepository : Repository<WorkerLaborTermInformation>, IWorkerLaborTermInformationRepository
    {
        public WorkerLaborTermInformationRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<WorkerLaborTermInformation>> GetAllWithManagementPosition()
        {
            return await _context.WorkerLaborTermInformations
                .Include(x=>x.Term)
                .Include(x=>x.WorkerManagementPosition)
                .Include(x => x.WorkerLaborCondition)
                .Include(x => x.WorkerLaborCategory)
                .Include(x => x.WorkerLaborRegime)
                .Where(x => x.WorkerManagementPosition != null)
                .ToListAsync();
        }

        public async Task<WorkerLaborTermInformation> GetByUserIdAndActiveTerm(string userId)
        {
            return await _context.WorkerLaborTermInformations.Where(x => x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
