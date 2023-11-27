using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces
{
    public interface IInternshipDevelopmentRepository : IRepository<InternshipDevelopment>
    {
        Task<List<InternshipDevelopment>> GetByIntershipValidationRequestId(Guid internshipValidationRequestId, byte? type = null);
    }
}
