using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeRegistrationRepository : IRepository<GradeRegistration>
    {
        Task<IEnumerable<GradeRegistration>> GetAllByFilter(Guid studentSectionId);
        Task<GradeRegistration> GetByFilters(Guid sectionId, Guid? evaluationId, string userId);
    }
}
