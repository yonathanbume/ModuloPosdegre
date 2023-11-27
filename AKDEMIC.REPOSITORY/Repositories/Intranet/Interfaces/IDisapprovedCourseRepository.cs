using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDisapprovedCourseRepository : IRepository<DisapprovedCourse>
    {
        Task<int> LoadDisapprovedCoursesJob(string careerCode);
    }
}
