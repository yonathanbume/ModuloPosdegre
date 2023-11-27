using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TeacherInformationService : ITeacherInformationService
    {
        private readonly ITeacherInformationRepository _teacherInformationRepository;
        public TeacherInformationService(ITeacherInformationRepository teacherInformationRepository)
        {
            _teacherInformationRepository = teacherInformationRepository;
        }

        Task<TeacherInformation> ITeacherInformationService.GetAsync(Guid id)
            => _teacherInformationRepository.Get(id);
    }
}