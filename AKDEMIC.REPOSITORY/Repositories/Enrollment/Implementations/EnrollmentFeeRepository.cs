using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentFeeRepository : Repository<EnrollmentFee>, IEnrollmentFeeRepository
    {
        public EnrollmentFeeRepository(AkdemicContext context) : base(context)
        {
        }

        //public async Task<List<EnrollmentFee>> GetAllByTerm(Guid termId)
        //    => await _context.EnrollmentFees.Where(x => x.TermId == termId).ToListAsync();

        public async Task<List<Select2Structs.Result>> GetAllSelect2Data()
            => await _context.EnrollmentFees.Select(x => new Select2Structs.Result { Id = x.Id, Text = $"{x.EnrollmentFeeTerm.StudentScale.Name} - {x.EnrollmentFeeTerm.Term.Name} - {x.Count} Cuota(s)" }).ToListAsync();

        public async Task<EnrollmentFee> GetByFilter(Guid enrollmentFeeTermId, int feeCount)
        {
            var result = await _context.EnrollmentFees
                .FirstOrDefaultAsync(x => x.EnrollmentFeeTermId == enrollmentFeeTermId && x.Count == feeCount);
            return result;
        }
    }
}
