using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICourseTypeRepository : IRepository<CourseType>
    {
        Task<IEnumerable<CourseTypeSelect2Template>> GetAcademicYearsSelect2Template();
        Task<object> GetCourseTypesJson();
        Task<CourseType> GetFirst();
    }
}