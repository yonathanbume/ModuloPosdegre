using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class FinancialExecutionRepository : Repository<FinancialExecution>, IFinancialExecutionRepository
    {
        public FinancialExecutionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<FinancialExecution>> GetByType(int type)
        {
            return await _context.FinancialExecutions.Include(x=>x.Details).Where(x => x.Type == type).ToListAsync();
        }
    }
}
