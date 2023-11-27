using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentShiftService
    {
        Task<bool> AnyByTerm(Guid termId);
        Task<EnrollmentShift> Get(Guid id);
        Task<EnrollmentShift> GetByTerm(Guid termId);
        Task Insert(EnrollmentShift enrollmentShift);
        Task Update(EnrollmentShift enrollmentShift);
        Task<EnrollmentShift> GetActiveTermEnrollmentShift();
    }
}
