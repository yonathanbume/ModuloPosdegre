using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseSyllabusTeacherService
    {
        Task<CourseSyllabusTeacher> GetByTeacherIdAndCourseSyllabusId(string teacherId, Guid courseSyllabusId);
        Task<List<CourseSyllabusTeacher>> GetByCourseSyllabusId(Guid courseSyllabusId);
        Task Insert(CourseSyllabusTeacher entity);
        Task Update(CourseSyllabusTeacher entity);
        Task Delete(CourseSyllabusTeacher entity);
        Task Add(CourseSyllabusTeacher entity);

    }
}
