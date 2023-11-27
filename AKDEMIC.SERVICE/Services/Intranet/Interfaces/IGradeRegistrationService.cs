using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeRegistrationService
    {
        Task<IEnumerable<GradeRegistration>> GetAll();
        Task<GradeRegistration> Get(Guid id);
        Task Insert(GradeRegistration gradeRegistration);
        Task Update(GradeRegistration gradeRegistration);
        Task DeleteById(Guid id);
        Task<IEnumerable<GradeRegistration>> GetAllByFilter(Guid studentSectionId);
        Task<GradeRegistration> GetByFilters(Guid sectionId, Guid? evaluationId, string userId);
    }
}
