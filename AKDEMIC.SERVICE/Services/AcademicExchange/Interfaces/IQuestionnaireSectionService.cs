using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IQuestionnaireSectionService
    {
        Task Insert(QuestionnaireSection entity);
        Task Delete(QuestionnaireSection entity);
        Task DeleteRange(IEnumerable<QuestionnaireSection> entites);
        Task<QuestionnaireSection> Get(Guid id);
        Task<IEnumerable<QuestionnaireSection>> GetDetailsByQuestionnaireId(Guid questionnaireId);
        Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId);
        Task<bool> AnyByTitle(Guid questionnaireId, string title);
        Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireSectionsByQuestionnaireId(Guid questionnaireId);
    }
}
