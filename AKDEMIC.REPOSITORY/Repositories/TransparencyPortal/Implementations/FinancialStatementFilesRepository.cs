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
    public class FinancialStatementFilesRepository : Repository<FinancialStatementFile>, IFinancialStatementFilesRepository
    {
        public FinancialStatementFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<FinancialStatementFile>> GetByFinancialStatementId(Guid id)
        {
            return await _context.FinancialStatementFiles.Where(x => x.FinancialStatementId == id).ToListAsync();
        }
    }
}
