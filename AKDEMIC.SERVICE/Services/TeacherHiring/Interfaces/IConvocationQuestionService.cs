using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationQuestionService
    {
        Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null);
        Task Insert(ConvocationQuestion entity);
        Task Update(ConvocationQuestion entity);
        Task<ConvocationQuestion> Get(Guid id);
        Task Delete(ConvocationQuestion entity);
        Task<IEnumerable<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId);
        Task DeleteRange(IEnumerable<ConvocationQuestion> questions);
    }
}
