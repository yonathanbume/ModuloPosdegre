using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationQuestionRepository : IRepository<ConvocationQuestion>
    {
        Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null);
        Task<IEnumerable<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId);
    }
}
