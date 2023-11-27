using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class QuestionnaireAnswerByUserService : IQuestionnaireAnswerByUserService
    {
        private readonly IQuestionnaireAnswerByUserRepository _questionnaireAnswerByUserRepository;

        public QuestionnaireAnswerByUserService(IQuestionnaireAnswerByUserRepository questionnaireAnswerByUserRepository)
        {
            _questionnaireAnswerByUserRepository = questionnaireAnswerByUserRepository;
        }

        public async Task DeleteRange(IEnumerable<QuestionnaireAnswerByUser> entities)
            => await _questionnaireAnswerByUserRepository.DeleteRange(entities);

        public async Task<IEnumerable<QuestionnaireAnswerByUser>> GetAllByPostulationId(Guid postulationId)
            => await _questionnaireAnswerByUserRepository.GetAllByPostulationId(postulationId);

        public async Task InsertRange(List<QuestionnaireAnswerByUser> answers)
            => await _questionnaireAnswerByUserRepository.InsertRange(answers);

    }
}
