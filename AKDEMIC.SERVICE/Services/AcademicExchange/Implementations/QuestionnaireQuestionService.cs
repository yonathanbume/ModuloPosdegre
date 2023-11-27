using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class QuestionnaireQuestionService : IQuestionnaireQuestionService
    {
        private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;

        public QuestionnaireQuestionService(IQuestionnaireQuestionRepository questionnaireQuestionRepository)
        {
            _questionnaireQuestionRepository = questionnaireQuestionRepository;
        }

        public async Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null)
            => await _questionnaireQuestionRepository.AnyByDescription(sectionId, description,ignoredId);

        public async Task Delete(QuestionnaireQuestion entity)
            => await _questionnaireQuestionRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<QuestionnaireQuestion> entities)
            => await _questionnaireQuestionRepository.DeleteRange(entities);

        public async Task<QuestionnaireQuestion> Get(Guid id)
            => await _questionnaireQuestionRepository.Get(id);

        public async Task<IEnumerable<QuestionnaireQuestion>> GetAllByQuestionnaireId(Guid questionnaireId)
            => await _questionnaireQuestionRepository.GetAllByQuestionnaireId(questionnaireId);

        public async Task<IEnumerable<QuestionnaireQuestion>> GetAllBySectionId(Guid sectionId)
            => await _questionnaireQuestionRepository.GetAllBySectionId(sectionId);

        public async Task<QuestionnaireQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle ,Guid scholarshipId)
            => await _questionnaireQuestionRepository.GetByDescriptionAndSectionTitle(description, sectionTitle, scholarshipId);

        public async Task Insert(QuestionnaireQuestion entity)
            => await _questionnaireQuestionRepository.Insert(entity);

        public async Task Update(QuestionnaireQuestion entity)
            => await _questionnaireQuestionRepository.Update(entity);
    }
}
