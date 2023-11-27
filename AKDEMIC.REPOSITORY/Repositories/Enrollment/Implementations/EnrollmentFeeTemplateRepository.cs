using AKDEMIC.CORE.Structs;
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
    public class EnrollmentFeeTemplateRepository : Repository<EnrollmentFeeTemplate>, IEnrollmentFeeTemplateRepository
    {
        public EnrollmentFeeTemplateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<EnrollmentFeeTemplate> GetByFeeCount(int feeCount)
        {
            var data = await _context.EnrollmentFeeTemplates.FirstOrDefaultAsync(x => x.Count == feeCount);
            return data;
        }
    }
}
