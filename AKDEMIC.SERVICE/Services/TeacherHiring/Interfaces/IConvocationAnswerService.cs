using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationAnswerService
    {
        Task<IEnumerable<ConvocationAnswer>> GetAllByQuestionId(Guid questionId);
        Task DeleteRange(IEnumerable<ConvocationAnswer> entities);
        Task<IEnumerable<ConvocationAnswer>> GetAllBySectionId(Guid sectionId);
    }
}
