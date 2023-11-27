using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherExperienceService
    {
        Task<object> GetAllAsModelA();
    }
}