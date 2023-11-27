using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IQuestionnaireAnswerService
    {
        Task DeleteRange(IEnumerable<QuestionnaireAnswer> entites);
        Task Delete(QuestionnaireAnswer entity);
        Task<IEnumerable<QuestionnaireAnswer>> GetAllBySectionId(Guid sectionId);
        Task<IEnumerable<QuestionnaireAnswer>> GetAllByQuestionId(Guid questionId);
    }
}
