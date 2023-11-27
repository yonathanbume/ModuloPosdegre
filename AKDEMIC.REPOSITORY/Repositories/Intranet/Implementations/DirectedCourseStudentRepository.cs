using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DirectedCourseStudentRepository : Repository<DirectedCourseStudent>, IDirectedCourseStudentRepository
    {
        public DirectedCourseStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<DirectedCourseStudent>Get(Guid id)
        {
            return await _context.DirectedCourseStudents
                .Where(x => x.Id == id)
                .Include(x => x.DirectedCourse.Term)
                .Include(x => x.DirectedCourse.Course)
                .Include(x => x.Student)
                .FirstOrDefaultAsync();
        }
        public async Task<DirectedCourseStudent> GetByFilters(Guid? studentId = null, Guid? directedcourseId = null, byte? status = null)
        {
            var query = _context.DirectedCourseStudents
                .Include(x=> x.DirectedCourse.Course)
                .AsQueryable();

            if (studentId.HasValue && studentId != Guid.Empty)
                query = query.Where(x => x.StudentId == studentId);

            if (directedcourseId.HasValue && directedcourseId != Guid.Empty)
                query = query.Where(x => x.DirectedCourseId == directedcourseId);

            if (status.HasValue) query = query.Where(x => x.Status == status);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null)
        {
            var query = _context.DirectedCourseStudents.AsQueryable();

            if (studentId.HasValue && studentId != Guid.Empty) 
                query = query.Where(x => x.StudentId == studentId);

            if (courseId.HasValue && courseId != Guid.Empty) 
                query = query.Where(x => x.DirectedCourse.CourseId == courseId);

            return await query.CountAsync();
        }

        public async Task<object> GetStudentsByCourseAndTerm(Guid id, Guid termId)
        {
            return await _context.DirectedCourseStudents
                    .Where(x => x.DirectedCourse.CourseId == id && x.DirectedCourse.TermId == termId)
                    .Select(x => new
                    {
                        id = x.Id,
                        code = x.Student.User.UserName,
                        name = x.Student.User.FullName,
                        program = x.Student.AcademicProgramId.HasValue ? x.Student.AcademicProgram.Name : "---",
                        academicYear = x.Student.CurrentAcademicYear
                    })
                    .OrderBy(x => x.name)
                    .ToListAsync();
        }
    }
}
