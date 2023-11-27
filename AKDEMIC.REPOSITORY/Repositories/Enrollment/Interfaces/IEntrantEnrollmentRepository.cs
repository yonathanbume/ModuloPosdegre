using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEntrantEnrollmentRepository : IRepository<EntrantEnrollment>
    {
        Task<IEnumerable<EntrantEnrollment>> GetAllWithData(Guid careerId, Guid? termId = null);
        Task<EntrantEnrollment> GetEnrollmentByTermidAndCareerId(Guid careerId, Guid? termId = null);
    }
}
