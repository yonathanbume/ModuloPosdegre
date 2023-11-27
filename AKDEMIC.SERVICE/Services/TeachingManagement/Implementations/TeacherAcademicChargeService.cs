using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class TeacherAcademicChargeService : ITeacherAcademicChargeService
    {
        private readonly ITeacherAcademicChargeRepository _teacherAcademicChargeRepository;

        public TeacherAcademicChargeService(ITeacherAcademicChargeRepository teacherAcademicChargeRepository)
        {
            _teacherAcademicChargeRepository = teacherAcademicChargeRepository;
        }

        public async Task<TeacherAcademicCharge> Get(Guid id)
            => await _teacherAcademicChargeRepository.Get(id);

        public async Task<bool> HasAcademicChargeValidated(Guid termId, string teacherId)
            => await _teacherAcademicChargeRepository.HasAcademicChargeValidated(termId, teacherId);

        public async Task Insert(TeacherAcademicCharge teacherAcademicCharge)
            => await _teacherAcademicChargeRepository.Insert(teacherAcademicCharge);

        public async Task Update(TeacherAcademicCharge teacherAcademicCharge)
            => await _teacherAcademicChargeRepository.Update(teacherAcademicCharge);


    }
}
