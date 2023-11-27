using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DisapprovedCourseService: IDisapprovedCourseService
    {
        private readonly IDisapprovedCourseRepository _disapprovedCourseRepository;
        public DisapprovedCourseService(IDisapprovedCourseRepository disapprovedCourseRepository)
        {
            _disapprovedCourseRepository = disapprovedCourseRepository;
        }

        public async Task<int> LoadDisapprovedCoursesJob(string careerCode)
        {
            return await _disapprovedCourseRepository.LoadDisapprovedCoursesJob(careerCode);
        }
    }
}
