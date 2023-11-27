using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Implementations
{
    public class InternshipDevelopmentRepository : Repository<InternshipDevelopment>, IInternshipDevelopmentRepository
    {
        public InternshipDevelopmentRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<InternshipDevelopment>> GetByIntershipValidationRequestId(Guid internshipValidationRequestId, byte? type = null)
        {
            var query = _context.InternshipDevelopments.Where(x => x.InternshipRequestId == internshipValidationRequestId);

            if (type.HasValue)
            {
                query = query.Where(x => x.InternshipAspect.Type == type);
            }

            return await query.ToListAsync();
        }
    }
}
