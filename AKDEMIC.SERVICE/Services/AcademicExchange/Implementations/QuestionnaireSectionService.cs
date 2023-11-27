using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class QuestionnaireSectionService : IQuestionnaireSectionService
    {
        private readonly IQuestionnaireSectionRepository _questionnaireSectionRepository;

        public QuestionnaireSectionService(IQuestionnaireSectionRepository questionnaireSectionRepository)
        {
            _questionnaireSectionRepository = questionnaireSectionRepository;
        }

        public async Task<bool> AnyByTitle(Guid questionnaireId, string title)
            => await _questionnaireSectionRepository.AnyByTitle(questionnaireId, title);

        public async Task Delete(QuestionnaireSection entity)
            => await _questionnaireSectionRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<QuestionnaireSection> entites)
            => await _questionnaireSectionRepository.DeleteRange(entites);

        public async Task<QuestionnaireSection> Get(Guid id)
            => await _questionnaireSectionRepository.Get(id);

        public async Task<IEnumerable<QuestionnaireSection>> GetDetailsByQuestionnaireId(Guid questionnaireId)
            => await _questionnaireSectionRepository.GetDetailsByQuestionnaireId(questionnaireId);

        public async Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId)
            => await _questionnaireSectionRepository.GetQuestionnaireDetailsByAcademicAgreementId(academicAgreementId);

        public async Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireSectionsByQuestionnaireId(Guid questionnaireId)
            => await _questionnaireSectionRepository.GetQuestionnaireSectionsByQuestionnaireId(questionnaireId);

        public async Task Insert(QuestionnaireSection entity)
            => await _questionnaireSectionRepository.Insert(entity);
    }
}
