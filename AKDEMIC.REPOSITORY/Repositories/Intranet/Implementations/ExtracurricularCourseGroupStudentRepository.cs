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
    public class ExtracurricularCourseGroupStudentRepository : Repository<ExtracurricularCourseGroupStudent>, IExtracurricularCourseGroupStudentRepository
    {
        public ExtracurricularCourseGroupStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyStudentInCourse(Guid studentId, Guid extracurricularCourseId)
            => await _context.ExtracurricularCourseGroupStudents
                .AnyAsync(x => x.StudentId == studentId && x.Group.ExtracurricularCourseId == extracurricularCourseId);

        public async Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByGroup(Guid groupId, byte? paymentStatus = null)
        {
            var query = _context.ExtracurricularCourseGroupStudents
                .Include(x => x.Student.User)
                .Where(x => x.GroupId == groupId)
                .AsQueryable();

            if (paymentStatus.HasValue)
                query = query.Where(x => x.Payment.Status == paymentStatus.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ExtracurricularCourseGroupStudent>> GetAllByStudent(Guid studentId)
            => await _context.ExtracurricularCourseGroupStudents
                .Include(x => x.Group.ExtracurricularCourse)
                .Where(x => x.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync();
    }
}
