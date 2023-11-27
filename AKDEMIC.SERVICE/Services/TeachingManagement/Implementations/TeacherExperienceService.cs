using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TeacherExperienceService : ITeacherExperienceService
    {
        private readonly ITeacherExperienceRepository _teacherExperienceRepository;
        public TeacherExperienceService(ITeacherExperienceRepository teacherExperienceRepository)
        {
            _teacherExperienceRepository = teacherExperienceRepository;
        }

        Task<object> ITeacherExperienceService.GetAllAsModelA()
            => _teacherExperienceRepository.GetAllAsModelA();
    }
}