using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerBankAccountInformationRepository : Repository<WorkerBankAccountInformation>, IWorkerBankAccountInformationRepository
    {
        public WorkerBankAccountInformationRepository(AkdemicContext context): base(context) { }

        public async Task<List<WorkerBankAccountInformation>> GetAllByWorker(Guid workerLaborInformationId)
        {
            var query = _context.WorkerBankAccountInformation.Where(x => x.WorkerLaborInformationId == workerLaborInformationId);

            return await query.ToListAsync();
        }
    }
}
