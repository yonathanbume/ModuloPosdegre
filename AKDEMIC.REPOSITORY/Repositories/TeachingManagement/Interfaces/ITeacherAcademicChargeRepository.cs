using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherAcademicChargeRepository : IRepository<TeacherAcademicCharge>
    {
        Task<bool> HasAcademicChargeValidated(Guid termId, string teacherId);
    }
}
