using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces
{
    public interface IInternshipDevelopmentService
    {
        Task<List<InternshipDevelopment>> GetByIntershipValidationRequestId(Guid internshipValidationRequestId, byte? type = null);
        Task InsertRange(List<InternshipDevelopment> entities);
        Task Insert(InternshipDevelopment entity);
        Task Update(InternshipDevelopment entity);
    }
}
