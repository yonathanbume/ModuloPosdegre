using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtracurricularCourseGroupAssistanceStudentRepository : Repository<ExtracurricularCourseGroupAssistanceStudent>, IExtracurricularCourseGroupAssistanceStudentRepository
    {
        public ExtracurricularCourseGroupAssistanceStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExtracurricularCourseGroupAssistanceStudent>> GetAllByAssistance(Guid assistanceId)
            => await _context.ExtracurricularCourseGroupAssistanceStudents
                .Where(x => x.GroupAssistanceId == assistanceId)
                .Include(x => x.GroupStudent.Student.User)
                .ToListAsync();
    }
}
