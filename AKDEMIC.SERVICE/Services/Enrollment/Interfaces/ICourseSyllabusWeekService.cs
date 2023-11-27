using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseSyllabusWeekService
    {
        Task<IEnumerable<CourseSyllabusWeek>> GetAllByCourseSyllabusId(Guid courseSyllabusId);
        Task Insert(CourseSyllabusWeek entity);
        Task InsertRange(IEnumerable<CourseSyllabusWeek> entities);
        Task UpdateRange(IEnumerable<CourseSyllabusWeek> entities);
        Task Update(CourseSyllabusWeek entity);

    }
}
