using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class DisapprovedCourseConceptRepository : Repository<DisapprovedCourseConcept>, IDisapprovedCourseConceptRepository
    {
        public DisapprovedCourseConceptRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<DisapprovedCourseConcept>> GetAll()
        {
            var result = await _context.DisapprovedCourseConcepts
                .Include(x => x.Concept)
                .Include(x => x.AdmissionType)
                .ToListAsync();
            return result;
        }
    }
}
