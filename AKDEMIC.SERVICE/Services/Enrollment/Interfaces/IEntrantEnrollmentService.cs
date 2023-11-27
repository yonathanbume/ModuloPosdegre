using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEntrantEnrollmentService
    {
        Task InsertEntrantEnrollment(EntrantEnrollment entrantEnrollment);
        Task UpdateEntrantEnrollment(EntrantEnrollment entrantEnrollment);
        Task DeleteEntrantEnrollment(EntrantEnrollment entrantEnrollment);
        Task<EntrantEnrollment> GetEntrantEnrollmentById(Guid id);
        Task<IEnumerable<EntrantEnrollment>> GetAllEntrantEnrollments();
        Task<IEnumerable<EntrantEnrollment>> GetAllWithData(Guid careerId, Guid? termId = null);
        Task<EntrantEnrollment> GetEnrollmentByTermidAndCareerId(Guid careerId, Guid? termId = null);
    }
}
