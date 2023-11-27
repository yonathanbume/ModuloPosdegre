using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseType;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class CourseTypeRepository : Repository<CourseType>, ICourseTypeRepository
    {
        public CourseTypeRepository(AkdemicContext context) : base(context) { }

        async Task<IEnumerable<CourseTypeSelect2Template>> ICourseTypeRepository.GetAcademicYearsSelect2Template()
        {
            var courseTypes = await _context.CourseTypes
                                 .Select(x => new CourseTypeSelect2Template
                                 {
                                     Id = x.Id,
                                     Text = x.Name
                                 }).ToListAsync();

            return courseTypes;
        }

        public async Task<object> GetCourseTypesJson()
        {
            var result = await _context.CourseTypes.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            return result;
        }

        public async Task<CourseType> GetFirst()
        {
            return await _context.CourseTypes.FirstOrDefaultAsync();
        }
    }
}