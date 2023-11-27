using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeacherAcademicChargeRepository : Repository<TeacherAcademicCharge> , ITeacherAcademicChargeRepository
    {
        public TeacherAcademicChargeRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> HasAcademicChargeValidated(Guid termId, string teacherId)
            => await _context.TeacherAcademicCharges.AnyAsync(y => y.TermId == termId && y.TeacherId == teacherId && y.IsValidated);
    }
}
