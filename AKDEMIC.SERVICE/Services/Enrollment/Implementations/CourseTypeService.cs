using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseType;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class CourseTypeService : ICourseTypeService
    {
        private readonly ICourseTypeRepository _courseTypeRepository;
        public CourseTypeService(ICourseTypeRepository courseTypeRepository)
        {
            _courseTypeRepository = courseTypeRepository;
        }

        Task<IEnumerable<CourseTypeSelect2Template>> ICourseTypeService.GetAcademicYearsSelect2Template()
            => _courseTypeRepository.GetAcademicYearsSelect2Template();

        public async Task<object> GetCourseTypesJson()
            => await _courseTypeRepository.GetCourseTypesJson();

        public async Task<CourseType> GetFirst()
        {
            return await _courseTypeRepository.GetFirst();
        }
    }
}