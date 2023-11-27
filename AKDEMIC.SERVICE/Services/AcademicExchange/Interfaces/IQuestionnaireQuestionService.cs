using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IQuestionnaireQuestionService
    {
        Task<QuestionnaireQuestion> Get(Guid id);
        Task Update(QuestionnaireQuestion entity);
        Task Insert(QuestionnaireQuestion entity);
        Task Delete(QuestionnaireQuestion entity);
        Task<IEnumerable<QuestionnaireQuestion>> GetAllBySectionId(Guid sectionId);
        Task DeleteRange(IEnumerable<QuestionnaireQuestion> entities);
        Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null);
        Task<IEnumerable<QuestionnaireQuestion>> GetAllByQuestionnaireId(Guid questionnaireId);
        Task<QuestionnaireQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle,Guid scholarshipId);
    }
}
