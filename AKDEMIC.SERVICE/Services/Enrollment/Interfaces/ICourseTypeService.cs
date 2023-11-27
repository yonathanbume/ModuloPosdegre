using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseTypeService
    {
        Task<IEnumerable<CourseTypeSelect2Template>> GetAcademicYearsSelect2Template();
        Task<object> GetCourseTypesJson();
        Task<CourseType> GetFirst();
    }
}