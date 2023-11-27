using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Implementations
{
    public class ConvocationComiteeRepository : Repository<ConvocationComitee> , IConvocationComiteeRepository
    {
        public ConvocationComiteeRepository(AkdemicContext context) : base(context) { }

        public async Task<ConvocationComitee> Get(Guid convocationId, string userId)
            => await _context.ConvocationComitees.Where(x => x.ConvocationId == convocationId && x.UserId == userId).FirstOrDefaultAsync();

        public async Task<List<ConvocationComitee>> GetComitee(Guid convocationId)
            => await _context.ConvocationComitees.Where(x => x.ConvocationId == convocationId).Include(x=>x.User).ToListAsync();
    }
}
