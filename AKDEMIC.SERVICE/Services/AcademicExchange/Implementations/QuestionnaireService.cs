using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;

        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        public async Task Delete(Questionnaire entity)
            => await _questionnaireRepository.Delete(entity);

        public async Task<Questionnaire> Get(Guid id)
            => await _questionnaireRepository.Get(id);

        public async Task<Questionnaire> GetByScholarshipId(Guid scholarshipId)
            => await _questionnaireRepository.GetByScholarshipId(scholarshipId);

        public async Task Insert(Questionnaire model)
            => await _questionnaireRepository.Insert(model);

        public async Task Update(Questionnaire entity)
            => await _questionnaireRepository.Update(entity);
    }
}
