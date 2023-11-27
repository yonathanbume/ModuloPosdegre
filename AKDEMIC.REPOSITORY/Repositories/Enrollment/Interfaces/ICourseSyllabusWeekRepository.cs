using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseSyllabusWeekRepository : IRepository<CourseSyllabusWeek>
    {
        Task<IEnumerable<CourseSyllabusWeek>> GetAllByCourseSyllabusId(Guid courseSyllabusId);
    }
}
