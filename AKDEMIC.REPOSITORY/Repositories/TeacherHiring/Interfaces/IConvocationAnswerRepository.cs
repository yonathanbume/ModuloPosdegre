using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationAnswerRepository : IRepository<ConvocationAnswer>
    {
        Task<IEnumerable<ConvocationAnswer>> GetAllByQuestionId(Guid questionId);
        Task<IEnumerable<ConvocationAnswer>> GetAllBySectionId(Guid sectionId);
    }
}
