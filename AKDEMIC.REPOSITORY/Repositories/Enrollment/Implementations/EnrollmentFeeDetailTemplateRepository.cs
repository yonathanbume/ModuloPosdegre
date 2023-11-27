using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentFeeDetailTemplateRepository : Repository<EnrollmentFeeDetailTemplate>, IEnrollmentFeeDetailTemplateRepository
    {
        public EnrollmentFeeDetailTemplateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<EnrollmentFeeDetailTemplate>> GetByFeeCount(int feeCount)
        {
            var data = await _context.EnrollmentFeeDetailTemplates
                .Where(x => x.EnrollmentFeeTemplate.Count == feeCount)
                .OrderBy(x => x.FeeNumber)
                .ToListAsync();

            return data;
        }
    }
}
