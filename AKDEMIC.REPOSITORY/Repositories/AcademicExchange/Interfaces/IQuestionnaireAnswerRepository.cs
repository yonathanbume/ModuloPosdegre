using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IQuestionnaireAnswerRepository : IRepository<QuestionnaireAnswer>
    {
        Task<IEnumerable<QuestionnaireAnswer>> GetAllBySectionId(Guid sectionId);
        Task<IEnumerable<QuestionnaireAnswer>> GetAllByQuestionId(Guid questionId);
    }
}
