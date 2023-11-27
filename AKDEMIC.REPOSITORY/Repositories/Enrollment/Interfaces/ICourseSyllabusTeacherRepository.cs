using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseSyllabusTeacherRepository : IRepository<CourseSyllabusTeacher>
    {
        Task<CourseSyllabusTeacher> GetByTeacherIdAndCourseSyllabusId(string teacherId, Guid courseSyllabusId);
        Task<List<CourseSyllabusTeacher>> GetByCourseSyllabusId(Guid courseSyllabusId);
    }
}
