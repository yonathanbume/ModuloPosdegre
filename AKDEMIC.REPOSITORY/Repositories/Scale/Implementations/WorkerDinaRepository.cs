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
    public class WorkerDinaRepository : Repository<WorkerDina>, IWorkerDinaRepository
    {
        public WorkerDinaRepository(AkdemicContext context) : base(context) { }

        public async Task<WorkerDina> GetByUserId(string userId)
            => await _context.WorkerDinas.Where(x => x.UserId == userId).Include(x => x.ExperienceAsThesisSupportMember).FirstOrDefaultAsync();

        public async Task<int> GetTeacherInDinaCount()
        {
            var count = await _context.WorkerDinas
                .Where(x => x.User.Teachers.Any(y => y.UserId == x.UserId))
                .Distinct()
                .CountAsync();

            return count;
        }
    }
}
