using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentShiftRepository : IRepository<EnrollmentShift>
    {
        Task<bool> AnyByTerm(Guid termId);
        Task<EnrollmentShift> GetByTerm(Guid termId);
        Task<EnrollmentShift> GetActiveTermEnrollmentShift();
    }
}
