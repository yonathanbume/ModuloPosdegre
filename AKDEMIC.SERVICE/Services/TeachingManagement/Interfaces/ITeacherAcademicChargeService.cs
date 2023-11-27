using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherAcademicChargeService
    {
        Task Insert(TeacherAcademicCharge teacherAcademicCharge);
        Task Update(TeacherAcademicCharge teacherAcademicCharge);
        Task<TeacherAcademicCharge> Get(Guid id);
        Task<bool> HasAcademicChargeValidated(Guid termId, string teacherId);
    }
}
