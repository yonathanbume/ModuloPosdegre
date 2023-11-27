using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamClassroomCareerRepository : Repository<AdmissionExamClassroomCareer>, IAdmissionExamClassroomCareerRepository
    {
        public AdmissionExamClassroomCareerRepository(AkdemicContext context) : base(context)
        {
        }
        public async Task DeleteByAdmissionExamClassroomId(Guid id)
        {
            var careers = await GetByAdmissionExamClassroomId(id);
            _context.AdmissionExamClassroomCareers.RemoveRange(careers);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdmissionExamClassroomCareer>> GetByAdmissionExamClassroomId(Guid id)
        {
            return await _context.AdmissionExamClassroomCareers.Where(x => x.AdmissionExamClassroomId == id).ToListAsync();
        }
    }
}
