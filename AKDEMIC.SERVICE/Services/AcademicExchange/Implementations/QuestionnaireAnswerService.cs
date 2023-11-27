using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class QuestionnaireAnswerService : IQuestionnaireAnswerService
    {
        private readonly IQuestionnaireAnswerRepository _questionnaireAnswerRepository;

        public QuestionnaireAnswerService(IQuestionnaireAnswerRepository questionnaireAnswerRepository)
        {
            _questionnaireAnswerRepository = questionnaireAnswerRepository;
        }

        public async Task Delete(QuestionnaireAnswer entity)
            => await _questionnaireAnswerRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<QuestionnaireAnswer> entites)
            => await _questionnaireAnswerRepository.DeleteRange(entites);

        public async Task<IEnumerable<QuestionnaireAnswer>> GetAllByQuestionId(Guid questionId)
            => await _questionnaireAnswerRepository.GetAllByQuestionId(questionId);

        public async Task<IEnumerable<QuestionnaireAnswer>> GetAllBySectionId(Guid sectionId)
            => await _questionnaireAnswerRepository.GetAllBySectionId(sectionId);
    }
}
