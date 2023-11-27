using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseSyllabusTeacherRepository : Repository<CourseSyllabusTeacher> , ICourseSyllabusTeacherRepository
    {
        public CourseSyllabusTeacherRepository(AkdemicContext context) : base(context) { }

        public async Task<CourseSyllabusTeacher> GetByTeacherIdAndCourseSyllabusId(string teacherId, Guid courseSyllabusId)
            => await _context.CourseSyllabusTeachers.Where(x => x.CourseSyllabusId == courseSyllabusId && x.TeacherId == teacherId).Include(x=>x.Teacher.User).FirstOrDefaultAsync();

        public async Task<List<CourseSyllabusTeacher>> GetByCourseSyllabusId(Guid courseSyllabusId)
            => await _context.CourseSyllabusTeachers.Where(x => x.CourseSyllabusId == courseSyllabusId).ToListAsync();
    }
}
