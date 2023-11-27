using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDisapprovedCourseService
    {
        Task<int> LoadDisapprovedCoursesJob(string careerCode);
    }
}
