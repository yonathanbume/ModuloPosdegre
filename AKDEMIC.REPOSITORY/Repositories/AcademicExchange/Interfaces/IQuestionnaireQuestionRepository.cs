using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IQuestionnaireQuestionRepository : IRepository<QuestionnaireQuestion>
    {
        Task<IEnumerable<QuestionnaireQuestion>> GetAllBySectionId(Guid sectionId);
        Task<bool> AnyByDescription(Guid sectionId, string description,Guid? ignoredId = null);
        Task<IEnumerable<QuestionnaireQuestion>> GetAllByQuestionnaireId(Guid questionnaireId);
        Task<QuestionnaireQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle, Guid scholarshipId);
    }
}
