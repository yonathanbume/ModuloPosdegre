using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IQuestionnaireSectionRepository : IRepository<QuestionnaireSection>
    {
        Task<IEnumerable<QuestionnaireSection>> GetDetailsByQuestionnaireId(Guid questionnaireId);
        Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId);
        Task<bool> AnyByTitle(Guid questionnaireId, string title);
        Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireSectionsByQuestionnaireId(Guid questionnaireId);
    }
}
