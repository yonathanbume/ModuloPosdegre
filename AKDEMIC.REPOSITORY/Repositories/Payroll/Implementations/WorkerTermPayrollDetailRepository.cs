using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkerTermPayrollDetailRepository : Repository<WorkerTermPayrollDetail>, IWorkerTermPayrollDetailRepository
    {
        public WorkerTermPayrollDetailRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTerm(Guid workingTermId)
        {
            return await _context.WorkerTermPayrollDetails.AnyAsync(x => x.WorkingTermId == workingTermId);
        }
    }
}
