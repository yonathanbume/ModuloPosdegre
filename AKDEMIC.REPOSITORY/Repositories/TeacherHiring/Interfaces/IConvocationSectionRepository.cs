using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationSectionRepository : IRepository<ConvocationSection>
    {
        Task<IEnumerable<ConvocationSection>> GetSectionsByConvocationId(Guid convocationId);
        Task<bool> AnyByTitle(Guid convocationId, string title, Guid? ignoredId = null);
    }
}
