using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class FinancialExecutionDetailsRepository : Repository<FinancialExecutionDetail>, IFinancialExecutionDetailsRepository
    {
        public FinancialExecutionDetailsRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<FinancialExecutionDetail>> GetByFinancialExecutionId(Guid id)
        {
            return await _context.FinancialExecutionDetails.Where(x => x.FinancialExecutionId == id).ToListAsync();
        }
    }
}
